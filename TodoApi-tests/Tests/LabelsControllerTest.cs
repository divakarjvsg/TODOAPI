using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoApi_tests
{
    public class LabelsControllerTest
    {
        private Mock<ILabelRepository> _labelRepository;
        private readonly Labels _label = new Labels { LabelId = 1, LabelName = "test" };
        private readonly List<Labels> _labels = new List<Labels>();
        private LabelsController _labelsController;        
        
        [SetUp]
        public void setup()
        {
             _labelRepository = new Mock<ILabelRepository>();
            var _todolistrepository = new Mock<ITodoListRepository>();
            var _todoitemrepository = new Mock<ITodoItemsRepository>();
            var loggerStub = new Mock<ILogger<LabelsController>>();
            _labels.Add(new Labels { LabelId = 1004 });
            _labels.Add(new Labels { LabelId = 1005 });

            _labelsController = new LabelsController(_labelRepository.Object, _todoitemrepository.Object, _todolistrepository.Object, loggerStub.Object);
            _labelRepository.Setup(p => p.AddLabels(It.IsAny<Labels>())).Returns(Task.FromResult(_label));
            _labelRepository.Setup(p => p.GetLabel(It.IsAny<int>())).Returns(Task.FromResult(_label));
            _labelRepository.Setup(p => p.DeleteLabel(It.IsAny<int>())).Returns(Task.FromResult(1007));
            _labelRepository.Setup(p => p.AssignLabel(It.IsAny<Guid>(), _labels)).Returns(Task.FromResult(_labels));
            _todolistrepository.Setup(p => p.GetTodoListByGuid(It.IsAny<Guid>())).Returns(Task.FromResult(new TodoLists { }));
            _todoitemrepository.Setup(p => p.GetTodoItemByGuid(It.IsAny<Guid>())).Returns(Task.FromResult(new TodoItems { }));
        }

        [Test]
        public async Task AssignLabeltoListTest()
        {
            var result = await _labelsController.AssignLabelstoList(new Guid("2ac5921c-fb30-4380-b6a1-d449517f2430"),_labels);
            Assert.IsNotNull(result);
            Assert.AreEqual("Labels Assigned to List", ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value);
        }

        [Test]
        public async Task AssignLabeltoItemTest()
        {
            var result = await _labelsController.AssignLabelstoItem(new Guid("de334ffa-efa8-436b-bf16-438e00cf030c"), _labels);
            Assert.IsNotNull(result);
            Assert.AreEqual("Labels Assigned to Item", ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value);
        }

        [Test]
        public async Task AddLabelTest()
        {
            var result = await _labelsController.AddLabels(new TodoAPI.Models.LabelModel() { LabelName = "test" });
            Assert.IsNotNull(result);            
            Assert.AreEqual(1, ((ToDoApi.Database.Models.Labels)((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value).LabelId);
        }


        [Test]
        public async Task DeleteLabelTest()
        {
            var result = await _labelsController.DeleteLabel(1007);
            Assert.IsNotNull(result);
            Assert.AreEqual("Label with Id = 1007 deleted", ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value);
        }
    }
}
