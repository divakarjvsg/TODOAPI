using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.Models;
using TodoAPI.Repositories;
using Xunit;

namespace TodoApi_tests
{
    public class LabelsControllerTest:ControllerBase
    {
        private readonly LabelsController _labelsController;
        private readonly ILabelRepository _labelRepository;
        public LabelsControllerTest()
        {
            _labelRepository = new LabelServiceFake();
            _labelsController = new LabelsController(_labelRepository,null,null,null);
        }

        [Fact]
        public void AssignLabel_ValidObjectPassed_ReturnsCreatedResponse() {

            // Arrange
            var SelectedGuid = new Guid("2da94280-7259-4fdb-bb2d-b0aae146a16b");
            List<Labels> SelectedLabels = new List<Labels>();
            
            SelectedLabels.Add(new Labels { LabelId=1003});
            SelectedLabels.Add(new Labels { LabelId = 4 });

            var result= _labelsController.AssignLabels(SelectedGuid, SelectedLabels);
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}
