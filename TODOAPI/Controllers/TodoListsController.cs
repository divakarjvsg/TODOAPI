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
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListRepository todoListRepository;

        public ILogger<TodoListsController> Logger { get; }

        public TodoListsController(ITodoListRepository todoListRepository,ILogger<TodoListsController> logger)
        {
            this.todoListRepository = todoListRepository;
            Logger = logger;
        }

        [Route("SearchList/{name}")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TodoLists>>> Search(string name)
        {
            try
            {
                var result = await todoListRepository.Search(name);

                if (result.Any())
                {
                    Logger.LogInformation($"Returned all TodoList data of Name: {name} from database.");
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in searching List item : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTodoLists([FromQuery] PageParmeters pageParmeters)
        {
            try
            {
                var result = await todoListRepository.GetTodoLists(pageParmeters);
                Logger.LogInformation($"Returned all TodoList data from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in fetching List items : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoLists>> GetTodoList(int id)
        {
            try
            {
                var result = await todoListRepository.GetTodoList(id);

                if (result == null)
                {
                    return NotFound();
                }
                Logger.LogInformation($"Returned TodoList ID={id} from database.");
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error in fetchin Listitem :{id} |" + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        [HttpPost]
        public async Task<ActionResult<TodoLists>> CreateTodoList(TodoLists todoList)
        {
            try
            {
                if (todoList == null)
                    return BadRequest();
                //todoList.CreatedDateTime = DateTime.Now;
                
                //todoList.ListGuid = Guid.NewGuid();
                var item = await todoListRepository.GetTodoListByName(todoList.TodoListName);

                if (item != null)
                {
                    ModelState.AddModelError("item", "Duplicate Item");
                    return BadRequest(ModelState);
                }

                var createdItem = await todoListRepository.AddTodoList(todoList);
                Logger.LogInformation($"Item created in TodoList with ID:{createdItem.Id}");

                return CreatedAtAction(nameof(GetTodoList),
                    new { id = createdItem.Id }, createdItem);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in creating Listitem : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new TodoList");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoLists>> UpdateTodoList(int id, TodoLists todoList)
        {
            try
            {
                if (id != todoList.Id)
                    return BadRequest("Item ID mismatch");

                var itemToUpdate = await todoListRepository.GetTodoList(id);

                if (itemToUpdate == null)
                {
                    return NotFound($"Item with Id = {id} not found");
                }
                Logger.LogInformation($"Item updated in TodoList with ID:{id}");
                return await todoListRepository.UpdateTodoList(todoList);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in updating Listitem : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating todoLists");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTodoList(int id)
        {
            try
            {
                var itemToDelete = await todoListRepository.GetTodoList(id);

                if (itemToDelete == null)
                {
                    return NotFound($"Item with Id = {id} not found");
                }

                await todoListRepository.DeleteTodoList(id);
                Logger.LogInformation($"Item deleted in TodoList with ID:{id}");
                return Ok($"Item with Id = {id} deleted");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error in deleting Listitem :{id} | " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting TodoList");
            }
        }
    }
}
