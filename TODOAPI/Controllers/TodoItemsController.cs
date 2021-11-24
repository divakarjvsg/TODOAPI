using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.Models.UpdateModels;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;
using ToDoAPI.Models.ResponseModels;

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

        /// <summary>
        /// Search for specific todoItem name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> returns specific todoitem records with details provided.</response>
        /// <response code="404"> A record with the specified name in items was not found.</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(TodoItems),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(status404error),StatusCodes.Status404NotFound)]
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
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all TodoItems
        /// </summary>
        /// <param name="pageParmeters"></param>
        /// <returns>Returns Action Result type based on Success or Failure. </returns>
        /// <response code="200"> Gets all todoitems records.</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(TodoItems), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GetTodoItems([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _todoItemsRepository.GetTodoItems(pageParmeters);
            _logger.LogInformation($"Items fetched from ToDoItems");
            return Ok(result);
        }

        /// <summary>
        /// Get specific todoItem 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> returns specific todoitem records with details provided.</response>
        /// <response code="404"> Error: Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(TodoItems), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(status404error), StatusCodes.Status404NotFound)]
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
            return Ok(result);
        }

        /// <summary>
        /// Create todoItem 
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="201"> returns todoitem created with details provided.</response>
        /// <response code="400"> Error:Bad Request</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(TodoItems), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(status400error), StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Update a todoitem
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="todoItem"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="404"> A record with the specified itemid was not found.</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="400"> Error : Bad Request</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(patchtodoitem404), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(itemidmismatch), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{itemId:int}")]
        [HttpPut]
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
            itemToUpdate.Id = todoItem.ListId;
            return await _todoItemsRepository.UpdateTodoItem(itemToUpdate);
        }

        /// <summary>
        /// Partial update an todoitem
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="todoItemPatchDoc"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="404">Error: Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="400"> itemid mismatch in the paramaters</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(TodoItemModel),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(patchtodoitem404),StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch]
        public async Task<ActionResult<TodoItems>> PatchTodoList([Required] int itemId, [FromBody] JsonPatchDocument<UpdateTodoItemModel> todoItemPatchDoc)
        {
            if (todoItemPatchDoc == null)
            {
                return BadRequest();
            }
            var itemToUpdate = await _todoItemsRepository.GetTodoItem(itemId);
            if (itemToUpdate == null)
            {
                return NotFound($"Item with Id = {itemId} not found in ToDoItem");
            }
            UpdateTodoItemModel updateTodoItem = new UpdateTodoItemModel { ListId = itemToUpdate.Id, ItemID = itemToUpdate.ItemID,ItemName=itemToUpdate.ItemName };
            todoItemPatchDoc.ApplyTo(updateTodoItem);
            bool isValid = TryValidateModel(updateTodoItem);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            itemToUpdate.ItemName = updateTodoItem.ItemName;
            itemToUpdate.Id = updateTodoItem.ListId;
            return await _todoItemsRepository.UpdateTodoItem(itemToUpdate);
        }

        /// <summary>
        /// Delete a todoItem
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> delete specific todoitem records with details provided.</response>
        /// <response code="404">Error : Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(patchtodoitem404),StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
