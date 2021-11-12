using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Interfaces;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelRepository labelRepository;
        private readonly ITodoItemsRepository todoItemsRepository;
        private readonly ITodoListRepository todoListRepository;

        public ILogger Logger { get; }

        public LabelsController(ILabelRepository labelRepository,ITodoItemsRepository todoItemsRepository,ITodoListRepository todoListRepository, ILogger<LabelsController> logger)
        {
            this.labelRepository = labelRepository;
            this.todoItemsRepository = todoItemsRepository;
            this.todoListRepository = todoListRepository;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetLabels()
        {
            try
            {                
                return Ok(await labelRepository.GetLabels());
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in GetLabels : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Labels>> AddLabels(Labels label)
        {
            try
            {
                if (label == null)
                    return BadRequest();

                var item = await labelRepository.GetLabelByName(label.LabelName);

                if (item != null)
                {
                    ModelState.AddModelError("Label", "Duplicate Label");
                    return BadRequest(ModelState);
                }

                var createdItem = await labelRepository.AddLabels(label);

                return CreatedAtAction(nameof(GetLabels),
                    new { id = createdItem.LabelId }, createdItem);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in Adding Labels : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Label");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteLabel(int id)
        {
            try
            {
                var labelToDelete = await labelRepository.GetLabel(id);

                if (labelToDelete == null)
                {
                    return NotFound($"Label with Id = {id} not found");
                }

                await labelRepository.DeleteLabel(id);

                return Ok($"Label with Id = {id} deleted");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in deleting label : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting Label");
            }
        }

        [Route("Assign/{SelectedGuid:Guid}")]
        [HttpPost]
        public async Task<ActionResult> AssignLabels(Guid SelectedGuid, List<Labels> SelectedLabels)
        {
            try
            {
                if (SelectedGuid == null)
                    return BadRequest();

                var item = await todoItemsRepository.GetTodoItemByGuid(SelectedGuid);

                if (item == null)
                {
                    var listitem= await todoListRepository.GetTodoListByGuid(SelectedGuid);
                    if(listitem==null)
                    {
                        Logger.LogInformation($"No item/list in database with unique ID:{SelectedGuid}");
                        return StatusCode(StatusCodes.Status204NoContent,
                                    "Invalid ItemID/ListID");
                    }
                }

                await labelRepository.AssignLabel(SelectedGuid,SelectedLabels);

                return Ok("Labels Assigned");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in Assigning labels : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Assigning Labels");
            }
        }

    }
}
