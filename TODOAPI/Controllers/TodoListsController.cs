using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.Models.UpdateModels;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListsRepository _todoListRepository;
        private readonly ILogger<TodoListsController> _logger;

        public TodoListsController(ITodoListsRepository todoListRepository, ILogger<TodoListsController> logger)
        {
            _todoListRepository = todoListRepository;
            _logger = logger;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("SearchList/{name}")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TodoLists>>> Search(string name)
        {
            var result = await _todoListRepository.Search(name);
            if (result.Any())
            {
                _logger.LogInformation($"Returned all ToDoList data of Name: {name} from database.");
                return Ok(result);
            }
            return NotFound();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GetTodoLists([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _todoListRepository.GetTodoLists(pageParmeters);
            _logger.LogInformation($"Returned all ToDoList data from database.");
            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{ListId:int}")]
        public async Task<ActionResult<TodoLists>> GetTodoList(int ListId)
        {
            var result = await _todoListRepository.GetTodoList(ListId);
            if (result == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"Returned ToDoList ID={ListId} from database.");
            return result;
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<TodoLists>> CreateTodoList(TodoListModel todoList)
        {
            if (todoList == null)
                return BadRequest();
            var item = await _todoListRepository.GetTodoListByName(todoList.TodoListName);
            if (item != null)
            {
                ModelState.AddModelError("item", "Duplicate Item in ToDoList");
                return BadRequest(ModelState);
            }
            var itemtoCreate = new TodoLists { TodoListName = todoList.TodoListName };
            var createdItem = await _todoListRepository.AddTodoList(itemtoCreate);
            _logger.LogInformation($"Item created in ToDoList with ID:{createdItem.Id}");
            return CreatedAtAction(nameof(GetTodoList), new { id = createdItem.Id }, createdItem);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{listId:int}")]
        [AcceptVerbs("PUT", "PATCH")]
        public async Task<ActionResult<TodoLists>> UpdateTodoList(int listId, UpdateTodoListModel todoList)
        {
            if (listId != todoList.Id)
                return BadRequest("Item ID mismatch");
            var itemToUpdate = await _todoListRepository.GetTodoList(listId);
            if (itemToUpdate == null)
            {
                return NotFound($"Item with Id = {listId} not found in ToDoList");
            }
            itemToUpdate.TodoListName = todoList.TodoListName;
            _logger.LogInformation($"Item updated in ToDoList with ID:{listId}");
            return await _todoListRepository.UpdateTodoList(itemToUpdate);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{listId:int}")]
        public async Task<ActionResult> DeleteTodoList(int listId)
        {
            var itemToDelete = await _todoListRepository.GetTodoList(listId);
            if (itemToDelete == null)
            {
                return NotFound($"Item with Id = {listId} not found in ToDoList");
            }
            await _todoListRepository.DeleteTodoList(listId);
            _logger.LogInformation($"Item deleted in ToDoList with ID:{listId}");
            return Ok($"Item with Id = {listId} deleted in ToDoList");
        }
    }
}
