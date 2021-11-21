using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;
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
        private readonly ITodoListRepository _todoListRepository;
        private readonly ILogger<TodoItemsController> Logger;

        public TodoItemsController(ITodoItemsRepository _todoItemsRepository, ITodoListRepository _todoListRepository, ILogger<TodoItemsController> logger)
        {
            this._todoItemsRepository = _todoItemsRepository;
            this._todoListRepository = _todoListRepository;
            Logger = logger;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("SearchItems/{name}")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TodoItems>>> Search(string name)
        {
            var result = await _todoItemsRepository.Search(name);
            if (result.Any())
            {
                Logger.LogInformation($"Item fetched in ToDoItems with name:{name}");
                return Ok(result);
            }
            return NotFound();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> GetTodoItems([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _todoItemsRepository.GetTodoItems(pageParmeters);
            Logger.LogInformation($"Items fetched from ToDoItems");
            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoItems>> GetTodoItem(int id)
        {
            var result = await _todoItemsRepository.GetTodoItem(id);
            if (result == null)
            {
                return NotFound();
            }
            Logger.LogInformation($"Item fetched in ToDoItems with ID:{id}");
            return result;
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            Logger.LogInformation($"Item created in ToDoItems with ID:{createdItem.ItemID}");

            return CreatedAtAction(nameof(GetTodoItem),
                new { id = createdItem.ItemID }, createdItem);
        }



        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id:int}")]
        [AcceptVerbs("PUT", "PATCH")]
        public async Task<ActionResult<TodoItems>> UpdateTodoItem(int id, TodoItems todoItem)
        {
            if (id != todoItem.ItemID)
                return BadRequest("Item ID mismatch");

            var itemToUpdate = await _todoItemsRepository.GetTodoItem(id);
            if (itemToUpdate == null)
            {
                return NotFound($"Item with Id = {id} not found");
            }
            Logger.LogInformation($"Item updated in ToDoItems with ID:{id}");

            return await _todoItemsRepository.UpdateTodoItem(todoItem);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTodoItem(int id)
        {
            var itemToDelete = await _todoItemsRepository.GetTodoItem(id);
            if (itemToDelete == null)
            {
                return NotFound($"Item with Id = {id} not found");
            }

            await _todoItemsRepository.DeleteTodoItem(id);
            Logger.LogInformation($"Item deleted in ToDoItems with ID:{id}");
            return Ok($"Item with Id = {id} deleted");
        }
    }
}
