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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GetLabels([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _labelRepository.GetLabels(pageParmeters);
            _logger.LogInformation($"Labels fetched");
            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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


        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("AssignLabeltoItem/{itemId:int}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabelstoItem(int itemId, List<AssignLabelsModel> selectedLabels)
        {
            var item = await _todoItemsRepository.GetTodoItem(itemId);
            if (item == null)
            {
                _logger.LogInformation($"No item in ToDoItems with ID:{itemId}");
                return StatusCode(StatusCodes.Status404NotFound, "Invalid ItemID");
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


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("AssignLabeltoList/{listId:int}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabelstoList(int listId, List<AssignLabelsModel> SelectedLabels)
        {
            var listitem = await _todoListRepository.GetTodoList(listId);
            if (listitem == null)
            {
                _logger.LogInformation($"No list in ToDoList with ID:{listId}");
                return StatusCode(StatusCodes.Status404NotFound, "Invalid ListID");
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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("LabelsAssignedtoList/{listId:int}")]
        [HttpGet]
        public async Task<ActionResult<LabelsListModel>> LabelsAssignedtoList(int listId)
        {
            var listitem = await _todoListRepository.GetTodoList(listId);
            if (listitem == null)
            {
                _logger.LogInformation($"No list in ToDoList with ListID:{listId}");
                return StatusCode(StatusCodes.Status404NotFound, "Invalid ListID");
            }
            LabelsListModel assignedLabels = new LabelsListModel
            {
                TodoList = listitem
            };
            var labels = await _labelRepository.GetLabelByGuid(listitem.ListGuid);
            assignedLabels.LabelsAssigned = labels;
            return Ok(assignedLabels);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("LabelsAssignedtoItem/{itemId:int}")]
        [HttpGet]
        public async Task<ActionResult<LabelsItemModel>> LabelsAssignedtoItem(int itemId)
        {
            var item = await _todoItemsRepository.GetTodoItem(itemId);
            if (item == null)
            {
                _logger.LogInformation($"No Item in ToDoItem with ItemId:{itemId}");
                return StatusCode(StatusCodes.Status404NotFound, "Invalid ItemId");
            }
            LabelsItemModel assignedLabels = new LabelsItemModel
            {
                TodoItem = item
            };
            var labels = await _labelRepository.GetLabelByGuid(item.ItemGuid);
            assignedLabels.LabelsAssigned = labels;
            return Ok(assignedLabels);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
