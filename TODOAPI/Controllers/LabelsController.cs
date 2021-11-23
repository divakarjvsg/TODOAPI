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
        private ILogger Logger { get; }

        public LabelsController(ILabelsRepository _labelRepository, ITodoItemsRepository _todoItemsRepository, ITodoListsRepository _todoListRepository, ILogger<LabelsController> logger)
        {
            this._labelRepository = _labelRepository;
            this._todoItemsRepository = _todoItemsRepository;
            this._todoListRepository = _todoListRepository;
            Logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GetLabels([FromQuery] PageParmeters pageParmeters)
        {
            var result = await _labelRepository.GetLabels(pageParmeters);
            Logger.LogInformation($"Labels fetched");
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("AssignLabeltoItem/{ItemId:int}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabelstoItem(int ItemId, List<AssignLabelsModel> SelectedLabels)
        {
            var item = await _todoItemsRepository.GetTodoItem(ItemId);
            if (item == null)
            {
                Logger.LogInformation($"No item in ToDoItems with ID:{ItemId}");
                return StatusCode(StatusCodes.Status204NoContent,
                            "Invalid ItemID");
            }
            List<Labels> TempLabels = new List<Labels>();
            foreach (var label in SelectedLabels)
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("AssignLabeltoList/{ListId:int}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabelstoList(int ListId, List<AssignLabelsModel> SelectedLabels)
        {
            var listitem = await _todoListRepository.GetTodoList(ListId);
            if (listitem == null)
            {
                Logger.LogInformation($"No list in ToDoList with ID:{ListId}");
                return StatusCode(StatusCodes.Status204NoContent,
                            "Invalid ListID");
            }
            List<Labels> TempLabels = new List<Labels>();
            foreach (var label in SelectedLabels)
            {
                var labelresult = await _labelRepository.GetLabel(label.LabelId);
                if (labelresult == null)
                    return StatusCode(StatusCodes.Status204NoContent,
                                "Invalid Label(s)");
                TempLabels.Add(labelresult);
            }
            await _labelRepository.AssignLabel(listitem.ListGuid, TempLabels);
            return Ok("Labels Assigned to List");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("LabelsAssignedtoList/{ListId:int}")]
        [HttpGet]
        public async Task<ActionResult<LabelsListModel>> LabelsAssignedtoList(int ListId)
        {
            var listitem = await _todoListRepository.GetTodoList(ListId);
            if (listitem == null)
            {
                Logger.LogInformation($"No list in ToDoList with ListID:{ListId}");
                return StatusCode(StatusCodes.Status204NoContent,
                            "Invalid ListID");
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("LabelsAssignedtoItem/{ItemId:int}")]
        [HttpGet]
        public async Task<ActionResult<LabelsItemModel>> LabelsAssignedtoItem(int ItemId)
        {
            var item = await _todoItemsRepository.GetTodoItem(ItemId);
            if (item == null)
            {
                Logger.LogInformation($"No Item in ToDoItem with ItemId:{ItemId}");
                return StatusCode(StatusCodes.Status204NoContent,
                            "Invalid ItemId");
            }
            LabelsItemModel assignedLabels = new LabelsItemModel
            {
                TodoItem = item
            };
            var labels = await _labelRepository.GetLabelByGuid(item.ItemGuid);
            assignedLabels.LabelsAssigned = labels;
            return Ok(assignedLabels);
        }
    }
}
