using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.Models.CreateModels;
using TodoAPI.Models.ResponseModels;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;
using ToDoAPI.Models.ResponseModels;

namespace TodoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelsRepository _labelRepository;
        private readonly ITodoItemsRepository _todoItemsRepository;
        private readonly ITodoListsRepository _todoListRepository;
        private ILogger _logger { get; }

        public LabelsController(ILabelsRepository labelRepository, ITodoItemsRepository todoItemsRepository, ITodoListsRepository todoListRepository, ILogger<LabelsController> logger)
        {
            _labelRepository = labelRepository;
            _todoItemsRepository = todoItemsRepository;
            _todoListRepository = todoListRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all labels
        /// </summary>
        /// <param name="pageParmeters"></param>
        /// <returns>Returns Action Result type based on Success or Failure. </returns>
        /// <response code="200"> Gets all label records.</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GetLabels([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _labelRepository.GetLabels(pageParmeters);
            _logger.LogInformation($"Labels fetched");
            return Ok(result);
        }

        /// <summary>
        /// Create a label
        /// </summary>
        /// <param name="label"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="201"> returns label created with details provided.</response>
        /// <response code="400"> Error : Bad Request</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(status400error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Labels),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("AddLabel")]
        [HttpPost]
        public async Task<ActionResult<Labels>> AddLabels(LabelModel label)
        {
            if (label == null)
                return BadRequest();

            var item = await _labelRepository.GetLabelByName(label.LabelName);
            if (item != null)
            {
                ModelState.AddModelError("Label", "Duplicate Label");
                return BadRequest(ModelState);
            }
            var labeltoCreate = new Labels { LabelName = label.LabelName };
            var createdItem = await _labelRepository.AddLabels(labeltoCreate);
            return CreatedAtAction(nameof(GetLabels),
                new { id = createdItem.LabelId }, createdItem);
        }

        /// <summary>
        /// Delete a label
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> delete specific label records with details provided.</response>
        /// <response code="404"> Error : Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(patchtodoitem404), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{labelId:int}")]
        public async Task<ActionResult> DeleteLabel(int labelId)
        {
            var labelToDelete = await _labelRepository.GetLabel(labelId);
            if (labelToDelete == null)
            {
                return NotFound($"Label with Id = {labelId} not found");
            }
            await _labelRepository.DeleteLabel(labelId);
            return Ok($"Label with Id = {labelId} deleted");
        }

        /// <summary>
        /// Assign labels to todoItem
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="selectedLabels"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> assigns labels to todoItem.</response>
        /// <response code="404"> Error : Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(invaliditem), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("AssignLabeltoItem/{itemId:int}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabelstoItem(int itemId, List<AssignLabelsModel> selectedLabels)
        {
            var item = await _todoItemsRepository.GetTodoItem(itemId);
            if (item == null)
            {
                _logger.LogInformation($"No item in ToDoItems with ID:{itemId}");
                return StatusCode(StatusCodes.Status404NotFound, "Invalid Id");
            }
            List<Labels> TempLabels = new List<Labels>();
            foreach (var label in selectedLabels)
            {
                var labelresult = await _labelRepository.GetLabel(label.LabelId);
                if (labelresult == null)
                    return StatusCode(StatusCodes.Status204NoContent,
                                "Invalid Label(s)");
                TempLabels.Add(labelresult);
            }
            await _labelRepository.AssignLabel(item.ItemGuid, TempLabels);
            return Ok("Labels Assigned to Item");
        }

        /// <summary>
        /// Assign labels to todoList
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="SelectedLabels"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> assigns labels to todoList.</response>
        /// <response code="404"> Error : Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(invaliditem), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("AssignLabeltoList/{listId:int}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabelstoList(int listId, List<AssignLabelsModel> SelectedLabels)
        {
            var listitem = await _todoListRepository.GetTodoList(listId);
            if (listitem == null)
            {
                _logger.LogInformation($"No list in ToDoList with ID:{listId}");
                return StatusCode(StatusCodes.Status404NotFound, "Invalid Id");
            }
            List<Labels> TempLabels = new List<Labels>();
            foreach (var label in SelectedLabels)
            {
                var labelresult = await _labelRepository.GetLabel(label.LabelId);
                if (labelresult == null)
                    return StatusCode(StatusCodes.Status404NotFound, "Invalid Label(s)");
                TempLabels.Add(labelresult);
            }
            await _labelRepository.AssignLabel(listitem.ListGuid, TempLabels);
            return Ok("Labels Assigned to List");
        }

        /// <summary>
        /// Get labels assigned to todoList
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> get assigned labels to todoList.</response>
        /// <response code="404"> Error: Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesResponseType(typeof(LabelsListModel),StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(invaliditem), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("LabelsAssignedtoList/{listId:int}")]
        [HttpGet]
        public async Task<ActionResult<LabelsListModel>> LabelsAssignedtoList(int listId)
        {
            var listitem = await _todoListRepository.GetTodoList(listId);
            if (listitem == null)
            {
                _logger.LogInformation($"No list in ToDoList with ListID:{listId}");
                return StatusCode(StatusCodes.Status404NotFound, "Invalid Id");
            }
            LabelsListModel assignedLabels = new LabelsListModel
            {
                TodoList = listitem
            };
            var labels = await _labelRepository.GetLabelByGuid(listitem.ListGuid);
            assignedLabels.LabelsAssigned = labels;
            return Ok(assignedLabels);
        }

        /// <summary>
        /// Get assigned labels to todoitem
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> get assigned labels to todoItem.</response>
        /// <response code="404"> Error : Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(invaliditem), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("LabelsAssignedtoItem/{itemId:int}")]
        [HttpGet]
        public async Task<ActionResult<LabelsItemModel>> LabelsAssignedtoItem(int itemId)
        {
            var item = await _todoItemsRepository.GetTodoItem(itemId);
            if (item == null)
            {
                _logger.LogInformation($"No Item in ToDoItem with ItemId:{itemId}");
                return StatusCode(StatusCodes.Status404NotFound, "Invalid Id");
            }
            LabelsItemModel assignedLabels = new LabelsItemModel
            {
                TodoItem = item
            };
            var labels = await _labelRepository.GetLabelByGuid(item.ItemGuid);
            assignedLabels.LabelsAssigned = labels;
            return Ok(assignedLabels);
        }

        /// <summary>
        /// Updates a label
        /// </summary>
        /// <param name="labelId"></param>
        /// <param name="labels"></param>
        /// <returns>Returns Action result type based on Success/Failure.</returns>
        /// <response code="200"> updated label</response>
        /// <response code="404"> Error : Not Found</response>
        /// <response code="401"> Error: Unauthorized</response>
        /// <response code="400"> Error : Bad Request</response>
        /// <response code="500"> some error occurred.</response>
        [ProducesErrorResponseType(typeof(LoginModel))]
        [ProducesResponseType(typeof(patchtodoitem404), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(status400error),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{labelId:int}")]
        public async Task<ActionResult> UpdateLabel(int labelId, [FromBody] Labels labels)
        {
            if (labelId != labels.LabelId)
                return BadRequest();
            var labelToUpdate = await _labelRepository.GetLabel(labelId);
            if (labelToUpdate == null)
            {
                return NotFound($"Label with Id = {labelId} not found");
            }
            await _labelRepository.UpdateLabels(labels);
            return Ok($"Label with Id = {labelId} Updated");
        }
    }
}
