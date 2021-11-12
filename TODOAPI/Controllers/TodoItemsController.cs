using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Interfaces;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsRepository todoItemsRepository;

        public ILogger<TodoItemsController> Logger { get; }

        public TodoItemsController(ITodoItemsRepository todoItemsRepository,ILogger<TodoItemsController> logger)
        {
            this.todoItemsRepository = todoItemsRepository;
            Logger = logger;
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<TodoItems>>> Search(string name)
        {
            try
            {
                var result = await todoItemsRepository.Search(name);

                if (result.Any())
                {
                    Logger.LogInformation($"Item fetched in Todoitems with name:{name}");
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in searching items : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTodoItems([FromQuery]PageParmeters pageParmeters)
        {
            try
            {
                var result = await todoItemsRepository.GetTodoItems(pageParmeters);
                Logger.LogInformation($"Items fetched from Todoitems");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in fetching items : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoItems>> GetTodoItem(int id)
        {
            try
            {
                var result = await todoItemsRepository.GetTodoItem(id);

                if (result == null)
                {
                    return NotFound();
                }
                Logger.LogInformation($"Item fetched in Todoitems with ID:{id}");
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error in fetching item :{id} || " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        [HttpPost]
        public async Task<ActionResult<TodoItems>> CreateTodoItem(TodoItems todoItem)
        {
            try
            {
                if (todoItem == null)
                    return BadRequest();

                var item = await todoItemsRepository.GetTodoItemByName(todoItem.ItemName);

                if (item != null)
                {
                    ModelState.AddModelError("item", "Duplicate Item");
                    return BadRequest(ModelState);
                }

                var createdItem= await todoItemsRepository.AddTodoItem(todoItem);
                Logger.LogInformation($"Item created in Todoitems with ID:{createdItem.ItemID}");
                return CreatedAtAction(nameof(GetTodoItem),
                    new { id = createdItem.ItemID }, createdItem);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in creating item : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Item");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoItems>> UpdateTodoItem(int id, TodoItems todoItem)
        {
            try
            {
                if (id != todoItem.ItemID)
                    return BadRequest("Item ID mismatch");

                var itemToUpdate = await todoItemsRepository.GetTodoItem(id);

                if (itemToUpdate == null)
                {
                    return NotFound($"Item with Id = {id} not found");
                }
                Logger.LogInformation($"Item updated in Todoitems with ID:{id}");
                return await todoItemsRepository.UpdateTodoItem(todoItem);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in updating item : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating Item");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTodoItem(int id)
        {
            try
            {
                var itemToDelete = await todoItemsRepository.GetTodoItem(id);

                if (itemToDelete == null)
                {
                    return NotFound($"Item with Id = {id} not found");
                }

                await todoItemsRepository.DeleteTodoItem(id);
                Logger.LogInformation($"Item deleted in Todoitems with ID:{id}");
                return Ok($"Item with Id = {id} deleted");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in deleting item : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting Item");
            }
        }
    }
}
