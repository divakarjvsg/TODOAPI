using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.DataAccess.Repositories;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoApi_tests.RepositoryTests
{
    class TodoListRepositoryTest : ToDoDbContextInitiator
    {
        private Mock<ITodoListsRepository> _todolistRepository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private ITodoListsRepository _todoListContract;
        private readonly TodoLists _todoList = new TodoLists { TodoListName = "test", Id = 7 };
        private static readonly ClaimsPrincipal user = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[] { new Claim("MyClaim", "3f14083e-c50b-4051-a445-18cee883323f") },
                            "Basic")
                        );
        [SetUp]
        public void Setup()
        {
            _todolistRepository = new Mock<ITodoListsRepository>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(h => h.HttpContext.User).Returns(user);
            _todoListContract = new TodoListsRepository(DBContext, _httpContextAccessor.Object);
            _todolistRepository.Setup(p => p.AddTodoList(It.IsAny<TodoLists>())).Returns(Task.FromResult(_todoList));
            _todolistRepository.Setup(p => p.DeleteTodoList(It.IsAny<int>())).Returns(Task.FromResult(1));
            _todolistRepository.Setup(p => p.GetTodoList(It.IsAny<int>())).Returns(Task.FromResult(_todoList));
            _todolistRepository.Setup(p => p.UpdateTodoList(It.IsAny<TodoLists>())).Returns(Task.FromResult(_todoList));
        }

        [Test]
        public async Task AddTodoListTest()
        {
            TodoLists result = await _todoListContract.AddTodoList(new TodoLists() { TodoListName = "testforresult" });
            Assert.IsNotNull(result);
            Assert.AreEqual("testforresult", result.TodoListName);
        }

        [Test]
        public async Task UpdateTodoListTest()
        {
            TodoLists result = await _todoListContract.UpdateTodoList(new TodoLists() { TodoListName = "testforresult", Id = 1010 });
            Assert.IsNotNull(result);
            Assert.AreEqual("testforresult", result.TodoListName);
        }

        [Test]
        public async Task DeleteLabelTest()
        {
            TodoLists result = await _todoListContract.GetTodoList(2016);
            await _todoListContract.DeleteTodoList(2006);
            Assert.AreEqual(2016, result.Id);
        }

    }
}
