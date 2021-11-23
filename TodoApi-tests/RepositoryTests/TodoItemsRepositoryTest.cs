using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.DataAccess.Repositories;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoApi_tests.RepositoryTests
{
    class TodoItemsRepositoryTest : ToDoDbContextInitiator
    {
        private Mock<ITodoItemsRepository> _todoitemsRepository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private ITodoItemsRepository _todoItemsContract;
        private readonly TodoItems _todoItem = new TodoItems { ItemID = 1, ItemName = "test", Id = 7 };

        private static readonly ClaimsPrincipal user =
            new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("MyClaim", "3f14083e-c50b-4051-a445-18cee883323f") }, "Basic"));

        [SetUp]
        public void Setup()
        {
            _todoitemsRepository = new Mock<ITodoItemsRepository>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(h => h.HttpContext.User).Returns(user);
            _todoItemsContract = new TodoItemsRepository(DBContext, _httpContextAccessor.Object);
            _todoitemsRepository.Setup(p => p.AddTodoItem(It.IsAny<TodoItems>())).Returns(Task.FromResult(_todoItem));
            _todoitemsRepository.Setup(p => p.DeleteTodoItem(It.IsAny<int>())).Returns(Task.FromResult(1));
            _todoitemsRepository.Setup(p => p.GetTodoItem(It.IsAny<int>())).Returns(Task.FromResult(_todoItem));
            _todoitemsRepository.Setup(p => p.UpdateTodoItem(It.IsAny<TodoItems>())).Returns(Task.FromResult(_todoItem));
        }

        [Test]
        public async Task AddTodoItemTest()
        {
            TodoItems result = await _todoItemsContract.AddTodoItem(new TodoItems() { ItemName = "testforresult", Id = 7 });
            Assert.IsNotNull(result);
            Assert.AreEqual("testforresult", result.ItemName);
        }

        [Test]
        public async Task UpdateTodoItemTest()
        {
            TodoItems result = await _todoItemsContract.UpdateTodoItem(new TodoItems() { ItemName = "testforresult", ItemID = 1010 });
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Now.ToString("dd/MM/yyyy"), result.UpdatedDate.ToString("dd/MM/yyyy"));
        }

        [Test]
        public async Task DeleteLabelTest()
        {
            TodoItems result = await _todoItemsContract.GetTodoItem(2016);
            await _todoItemsContract.DeleteTodoItem(2006);
            Assert.AreEqual(2016, result.ItemID);
        }
    }
}
