using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoAPI.Models;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]    
    public class LabelsController : ControllerBase
    {
        private readonly ILabelRepository _labelRepository;
        private readonly ITodoItemsRepository _todoItemsRepository;
        private readonly ITodoListRepository _todoListRepository;

        private ILogger Logger { get; }

        public LabelsController(ILabelRepository _labelRepository, ITodoItemsRepository _todoItemsRepository, ITodoListRepository _todoListRepository, ILogger<LabelsController> logger)
        {
            this._labelRepository = _labelRepository;
            this._todoItemsRepository = _todoItemsRepository;
            this._todoListRepository = _todoListRepository;
            Logger = logger;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> GetLabels([FromQuery] PageParmeters pageParmeters)
        {            
            var result = await _labelRepository.GetLabels(pageParmeters);
            Logger.LogInformation($"Labels fetched");
            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteLabel(int id)
        {
            var labelToDelete = await _labelRepository.GetLabel(id);
            if (labelToDelete == null)
            {
                return NotFound($"Label with Id = {id} not found");
            }

            await _labelRepository.DeleteLabel(id);

            return Ok($"Label with Id = {id} deleted");
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("AssignLabeltoItem/{SelectedGuid:Guid}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabelstoItem(Guid SelectedGuid, List<Labels> SelectedLabels)
        {
            if (SelectedGuid == null)
                return BadRequest();
            
            var item = await _todoItemsRepository.GetTodoItemByGuid(SelectedGuid);

            if (item == null)
            {
                Logger.LogInformation($"No item in ToDoItems with unique ID:{SelectedGuid}");
                return StatusCode(StatusCodes.Status204NoContent,
                            "Invalid ItemID");
            }

            foreach (var label in SelectedLabels)
            {
                var labelresult = await _labelRepository.GetLabel(label.LabelId);
                if (labelresult == null)
                    return StatusCode(StatusCodes.Status204NoContent,
                                "Invalid Label(s)");
            }

            await _labelRepository.AssignLabel(SelectedGuid, SelectedLabels);

            return Ok("Labels Assigned to Item");
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("AssignLabeltoList/{SelectedGuid:Guid}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabelstoList(Guid SelectedGuid, List<Labels> SelectedLabels)
        {
            if (SelectedGuid == null)
                return BadRequest();

            var listitem = await _todoListRepository.GetTodoListByGuid(SelectedGuid);

            if (listitem == null)
            {
                Logger.LogInformation($"No list in ToDoList with unique ID:{SelectedGuid}");
                return StatusCode(StatusCodes.Status204NoContent,
                            "Invalid ListID");
            }

            foreach (var label in SelectedLabels)
            {
                var labelresult = await _labelRepository.GetLabel(label.LabelId);
                if (labelresult == null)
                    return StatusCode(StatusCodes.Status204NoContent,
                                "Invalid Label(s)");
            }

            await _labelRepository.AssignLabel(SelectedGuid, SelectedLabels);

            return Ok("Labels Assigned to List");
        }
    }
}
