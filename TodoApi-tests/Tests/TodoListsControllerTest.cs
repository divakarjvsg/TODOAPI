using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.Models;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoApi_tests.Tests
{
    class TodoListsControllerTest
    {
        private Mock<ITodoListRepository> _todolistrepository;        
        private readonly TodoLists _todolist = new TodoLists { Id = 100, TodoListName = "test" };
        private TodoListsController _todoListsController;

        [SetUp]
        public void setup()
        {
            _todolistrepository = new Mock<ITodoListRepository>();
            var _loggerStub = new Mock<ILogger<TodoListsController>>();

            _todoListsController = new TodoListsController(_todolistrepository.Object, _loggerStub.Object);
            _todolistrepository.Setup(p => p.GetTodoList(It.IsAny<int>())).Returns(Task.FromResult(_todolist));
            _todolistrepository.Setup(p => p.AddTodoList(It.IsAny<TodoLists>())).Returns(Task.FromResult(_todolist));
            _todolistrepository.Setup(p => p.DeleteTodoList(It.IsAny<int>())).Returns(Task.FromResult(1007));
            _todolistrepository.Setup(p => p.UpdateTodoList(It.IsAny<TodoLists>())).Returns(Task.FromResult(_todolist));
        }

        [Test]
        public async Task AddTodoListTest()
        {
            var result = await _todoListsController.CreateTodoList(new TodoListModel() { TodoListName = "test" });
            Assert.IsNotNull(result);
            Assert.AreEqual(100, ((ToDoApi.Database.Models.TodoLists)((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value).Id);
        }

        [Test]
        public async Task DeleteTodoListTest()
        {
            var result = await _todoListsController.DeleteTodoList(1016);
            Assert.IsNotNull(result);
            Assert.AreEqual("Item with Id = 1016 deleted in ToDoList", ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value);
        }

        [Test]
        public async Task UpdateTodoListTest()
        {
            var result = await _todoListsController.UpdateTodoList(100, new TodoLists() { TodoListName = "test", Id = 100 });
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Value.Id);
        }

        [Test]
        public async Task GetTodoListTest()
        {
            var result = await _todoListsController.GetTodoList(100);
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Value.Id);
        }
    }
}
