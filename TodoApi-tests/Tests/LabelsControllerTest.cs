using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoApi_tests
{
    public class LabelsControllerTest
    {
        private Mock<ILabelRepository> _labelRepositorystub;
        //private ILabelRepository _labelContract;
        private readonly Labels _label = new Labels { LabelId = 1, LabelName = "test" };

        private LabelsController _labelsController;        
        
        [SetUp]
        public void setup()
        {
             _labelRepositorystub = new Mock<ILabelRepository>();
            var todolistrepositoryStub = new Mock<ITodoListRepository>();
            var todoitemrepositoryStub = new Mock<ITodoItemsRepository>();
            var loggerStub = new Mock<ILogger<LabelsController>>();

            _labelsController = new LabelsController(_labelRepositorystub.Object, todoitemrepositoryStub.Object, todolistrepositoryStub.Object, loggerStub.Object);
            _labelRepositorystub.Setup(p => p.AddLabels(It.IsAny<Labels>())).Returns(Task.FromResult(_label));
            _labelRepositorystub.Setup(p => p.GetLabel(It.IsAny<int>())).Returns(Task.FromResult(_label));
            _labelRepositorystub.Setup(p => p.DeleteLabel(It.IsAny<int>())).Returns(Task.FromResult(1007));
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
