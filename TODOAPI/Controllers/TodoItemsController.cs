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
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsRepository _todoItemsRepository;
        private readonly ITodoListsRepository _todoListRepository;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoItemsRepository todoItemsRepository, ITodoListsRepository todoListRepository, ILogger<TodoItemsController> logger)
        {
            _todoItemsRepository = todoItemsRepository;
            _todoListRepository = todoListRepository;
            _logger = logger;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("SearchItems/{name}")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TodoItems>>> Search(string name)
        {
            var result = await _todoItemsRepository.Search(name);
            if (result.Any())
            {
                _logger.LogInformation($"Item fetched in ToDoItems with name:{name}");
                return Ok(result);
            }
            return NotFound();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GetTodoItems([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _todoItemsRepository.GetTodoItems(pageParmeters);
            _logger.LogInformation($"Items fetched from ToDoItems");
            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{itemId:int}")]
        public async Task<ActionResult<TodoItems>> GetTodoItem(int itemId)
        {
            var result = await _todoItemsRepository.GetTodoItem(itemId);
            if (result == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"Item fetched in ToDoItems with ID:{itemId}");
            return result;
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<TodoItems>> CreateTodoItem(TodoItemModel todoItem)
        {
            if (todoItem == null)
                return BadRequest();
            var item = await _todoItemsRepository.GetTodoItemByName(todoItem.ItemName);
            if (item != null)
            {
                ModelState.AddModelError("item", "Duplicate Item");
                return BadRequest(ModelState);
            }
            var resultitem = await _todoListRepository.GetTodoList(todoItem.ListId);
            if (resultitem == null)
            {
                ModelState.AddModelError("item", "Invalid List Item");
                return BadRequest(ModelState);
            }
            var itemtoCreate = new TodoItems { ItemName = todoItem.ItemName, Id = todoItem.ListId };
            var createdItem = await _todoItemsRepository.AddTodoItem(itemtoCreate);
            _logger.LogInformation($"Item created in ToDoItems with ID:{createdItem.ItemID}");
            return CreatedAtAction(nameof(GetTodoItem),
                new { id = createdItem.ItemID }, createdItem);
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{itemId:int}")]
        [AcceptVerbs("PUT", "PATCH")]
        public async Task<ActionResult<TodoItems>> UpdateTodoItem(int itemId, UpdateTodoItemModel todoItem)
        {
            if (itemId != todoItem.ItemID)
                return BadRequest("Item ID mismatch");
            var itemToUpdate = await _todoItemsRepository.GetTodoItem(itemId);
            if (itemToUpdate == null)
            {
                return NotFound($"Item with Id = {itemId} not found");
            }
            _logger.LogInformation($"Item updated in ToDoItems with ID:{itemId}");
            itemToUpdate.ItemName = todoItem.ItemName;
            itemToUpdate.Id = todoItem.Id;
            return await _todoItemsRepository.UpdateTodoItem(itemToUpdate);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{itemId:int}")]
        public async Task<ActionResult> DeleteTodoItem(int itemId)
        {
            var itemToDelete = await _todoItemsRepository.GetTodoItem(itemId);
            if (itemToDelete == null)
            {
                return NotFound($"Item with Id = {itemId} not found");
            }
            await _todoItemsRepository.DeleteTodoItem(itemId);
            _logger.LogInformation($"Item deleted in ToDoItems with ID:{itemId}");
            return Ok($"Item with Id = {itemId} deleted");
        }
    }
}
