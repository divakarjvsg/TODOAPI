using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.Models;
using TodoAPI.Models.UpdateModels;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoApi_tests.Tests
{
    class TodoItemsControllerTest
    {
        private Mock<ITodoItemsRepository> _todoitemsrepository;
        private readonly TodoItems _todoitem = new TodoItems { ItemID = 100, Id = 1007, ItemName = "test" };
        private readonly TodoLists _todolist = new TodoLists { Id = 1007, TodoListName = "test" };
        private TodoItemsController _todoItemsController;

        [SetUp]
        public void SetUp()
        {
            _todoitemsrepository = new Mock<ITodoItemsRepository>();
            var todolistrepository = new Mock<ITodoListsRepository>();
            var loggerStub = new Mock<ILogger<TodoItemsController>>();

            _todoItemsController = new TodoItemsController(_todoitemsrepository.Object, todolistrepository.Object, loggerStub.Object);
            todolistrepository.Setup(p => p.GetTodoList(It.IsAny<int>())).Returns(Task.FromResult(_todolist));
            _todoitemsrepository.Setup(p => p.AddTodoItem(It.IsAny<TodoItems>())).Returns(Task.FromResult(_todoitem));
            _todoitemsrepository.Setup(p => p.DeleteTodoItem(It.IsAny<int>())).Returns(Task.FromResult(100));
            _todoitemsrepository.Setup(p => p.GetTodoItem(It.IsAny<int>())).Returns(Task.FromResult(_todoitem));
            _todoitemsrepository.Setup(p => p.UpdateTodoItem(It.IsAny<TodoItems>())).Returns(Task.FromResult(_todoitem));
        }

        [Test]
        public async Task AddTodoItemTest()
        {
            var result = await _todoItemsController.CreateTodoItem(new TodoItemModel() { ItemName = "test", ListId = 1007 });
            Assert.IsNotNull(result);
            Assert.AreEqual(100, ((ToDoApi.Database.Models.TodoItems)((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value).ItemID);
        }

        [Test]
        public async Task DeleteTodoItemTest()
        {
            var result = await _todoItemsController.DeleteTodoItem(1016);
            Assert.IsNotNull(result);
            Assert.AreEqual("Item with Id = 1016 deleted", ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value);
        }

        [Test]
        public async Task UpdateTodoItemTest()
        {
            var result = await _todoItemsController.UpdateTodoItem(100, new UpdateTodoItemModel() { ItemName = "test", ItemID = 100, ListId = 1007 });
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Value.ItemID);
        }

        [Test]
        public async Task GetTodoItemTest()
        {
            var result = await _todoItemsController.GetTodoItem(100);
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Value.ItemID);
        }

    }
}
