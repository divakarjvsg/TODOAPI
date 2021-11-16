using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.Interfaces;
using TodoAPI.Models;
using TodoAPI.Repositories;
using Xunit;

namespace TodoApi_tests
{
    public class LabelsControllerTest:ControllerBase
    {
        private readonly LabelsController _labelsController;
        
        public LabelsControllerTest()
        {
            var labelRepositorystub = new Mock<ILabelRepository>();
            var todolistrepositoryStub = new Mock<ITodoListRepository>();
            var todoitemrepositoryStub = new Mock<ITodoItemsRepository>();
            var loggerStub = new Mock<ILogger<LabelsController>>();

            _labelsController = new LabelsController(labelRepositorystub.Object, todoitemrepositoryStub.Object, todolistrepositoryStub.Object, loggerStub.Object);
        }

        [Fact]
        public async Task  AssignLabel_ValidObjectPassed_ReturnsNoContentResponse() {

            // Arrange
            var SelectedGuid = new Guid("2da94280-7259-4fdb-bb2d-b0aae146a16b");
            List<Labels> SelectedLabels = new List<Labels>();
            
            SelectedLabels.Add(new Labels { LabelId=1003});
            SelectedLabels.Add(new Labels { LabelId = 4 });
            //Act
            var result= await _labelsController.AssignLabels(SelectedGuid, SelectedLabels);
            
            //Assert
            Assert.IsType<ObjectResult>(result);
        }


        [Fact]
        public async Task AssignLabel_ValidObjectPassed_ReturnsCreatedResponse()
        {

            // Arrange
            var SelectedGuid = new Guid("230feef2-1065-4f4b-ba95-965907fe5916");
            List<Labels> SelectedLabels = new List<Labels>();

            SelectedLabels.Add(new Labels { LabelId = 1003 });
            SelectedLabels.Add(new Labels { LabelId = 4 });
            //Act
            var result = await _labelsController.AssignLabels(SelectedGuid, SelectedLabels);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
