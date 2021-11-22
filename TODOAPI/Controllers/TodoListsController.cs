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
        private readonly ITodoListRepository _todoListRepository;
        private readonly ILogger<TodoListsController> Logger;

        public TodoListsController(ITodoListRepository _todoListRepository, ILogger<TodoListsController> logger)
        {
            this._todoListRepository = _todoListRepository;
            Logger = logger;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("SearchList/{name}")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TodoLists>>> Search(string name)
        {
            var result = await _todoListRepository.Search(name);
            if (result.Any())
            {
                Logger.LogInformation($"Returned all ToDoList data of Name: {name} from database.");
                return Ok(result);
            }
            return NotFound();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> GetTodoLists([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _todoListRepository.GetTodoLists(pageParmeters);
            Logger.LogInformation($"Returned all ToDoList data from database.");
            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoLists>> GetTodoList(int id)
        {
            var result = await _todoListRepository.GetTodoList(id);
            if (result == null)
            {
                return NotFound();
            }
            Logger.LogInformation($"Returned ToDoList ID={id} from database.");
            return result;
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

            var itemtoCreate=new TodoLists{ TodoListName=todoList.TodoListName};
            var createdItem = await _todoListRepository.AddTodoList(itemtoCreate);
            Logger.LogInformation($"Item created in ToDoList with ID:{createdItem.Id}");

            return CreatedAtAction(nameof(GetTodoList),
                new { id = createdItem.Id }, createdItem);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id:int}")]
        [AcceptVerbs("PUT","PATCH")]
        public async Task<ActionResult<TodoLists>> UpdateTodoList(int id, UpdateTodoListModel todoList)
        {
            if (id != todoList.Id)
                return BadRequest("Item ID mismatch");

            var itemToUpdate = await _todoListRepository.GetTodoList(id);

            if (itemToUpdate == null)
            {
                return NotFound($"Item with Id = {id} not found in ToDoList");
            }
            itemToUpdate.TodoListName = todoList.TodoListName;
            Logger.LogInformation($"Item updated in ToDoList with ID:{id}");
            return await _todoListRepository.UpdateTodoList(itemToUpdate);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTodoList(int id)
        {
            var itemToDelete = await _todoListRepository.GetTodoList(id);
            if (itemToDelete == null)
            {
                return NotFound($"Item with Id = {id} not found in ToDoList");
            }

            await _todoListRepository.DeleteTodoList(id);

            Logger.LogInformation($"Item deleted in ToDoList with ID:{id}");
            return Ok($"Item with Id = {id} deleted in ToDoList");
        }
    }
}
