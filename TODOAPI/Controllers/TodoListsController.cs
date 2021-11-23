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
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListsRepository _todoListRepository;
        private readonly ILogger<TodoListsController> _logger;

        public TodoListsController(ITodoListsRepository todoListRepository, ILogger<TodoListsController> logger)
        {
            _todoListRepository = todoListRepository;
            _logger = logger;
        }

        /// <summary>
        /// Search for sepecific tolist with input
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> returns specific todolist records with details provided.</response>
        /// <response code="404"> A record with the specified name in items was not found.</response>
        /// <response code="401"> Authorization information is missing or invalid.</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesResponseType(typeof(TodoLists),StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Get all todolist
        /// </summary>
        /// <param name="pageParmeters"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> returns specific todolist records with details provided.</response>
        /// <response code="401"> Authorization information is missing or invalid.</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesResponseType(typeof(TodoLists), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GetTodoLists([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _todoListRepository.GetTodoLists(pageParmeters);
            _logger.LogInformation($"Returned all ToDoList data from database.");
            return Ok(result);
        }

        /// <summary>
        /// Get specifi todoList
        /// </summary>
        /// <param name="ListId"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> returns specific todolist records with details provided.</response>
        /// <response code="404"> A record with the specified Listid in todolist was not found.</response>
        /// <response code="401"> Authorization information is missing or invalid.</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates a todoList 
        /// </summary>
        /// <param name="todoList"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="201"> returns todoList created with details provided.</response>
        /// <response code="400"> todoList input was empty.</response>
        /// <response code="401"> Authorization information is missing or invalid.</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesResponseType(typeof(TodoLists),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(LoginModel))]
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

        /// <summary>
        /// updates a todoList 
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="todoList"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="404"> A record with the specified listid was not found.</response>
        /// <response code="401"> Authorization information is missing or invalid.</response>
        /// <response code="400"> listId mismatch in the paramaters</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesResponseType(typeof(UpdateTodoListModel),StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{listId:int}")]
        [HttpPut]
        public async Task<ActionResult<TodoLists>> UpdateTodoList(int listId, UpdateTodoListModel todoList)
        {
            if (listId != todoList.ListId)
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

        /// <summary>
        /// Partial update to todoList
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="todoListPatchDoc"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="400"> Input paramter is null/empty.</response>
        /// <response code="404"> A record with the specified todolist ID was not found.</response>
        /// <response code="401"> Authorization information is missing or invalid.</response>
        [ProducesResponseType(typeof(UpdateTodoListModel), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch]
        public async Task<ActionResult<TodoLists>> PatchTodoList([Required]int listId,[FromBody]JsonPatchDocument<UpdateTodoListModel> todoListPatchDoc) 
        {
            if (todoListPatchDoc == null)
            {
                return BadRequest();
            }
            var itemToUpdate = await _todoListRepository.GetTodoList(listId);
            if (itemToUpdate == null)
            {
                return NotFound($"Item with Id = {listId} not found in ToDoList");
            }
            UpdateTodoListModel updateTodoList = new UpdateTodoListModel { ListId = itemToUpdate.Id, TodoListName = itemToUpdate.TodoListName };
            todoListPatchDoc.ApplyTo(updateTodoList);
            bool isValid = TryValidateModel(updateTodoList);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            itemToUpdate.TodoListName = updateTodoList.TodoListName;
            return await _todoListRepository.UpdateTodoList(itemToUpdate);
        }

        /// <summary>
        /// Deletes a todoList
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> delete specific todolist records with details provided.</response>
        /// <response code="404"> A record with the specified listid was not found.</response>
        /// <response code="401"> Authorization information is missing or invalid.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(UpdateTodoListModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
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
