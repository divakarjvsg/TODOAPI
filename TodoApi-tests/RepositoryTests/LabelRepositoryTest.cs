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
    class LabelRepositoryTest : ToDoDbContextInitiator
    {
        private Mock<ILabelsRepository> _labelRepository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private ILabelsRepository _labelContract;
        private readonly Labels _label = new Labels { LabelId = 1, LabelName = "test" };
        private static readonly ClaimsPrincipal user = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[] { new Claim("MyClaim", "3f14083e-c50b-4051-a445-18cee883323f") },
                            "Basic")
                        );

        [SetUp]
        public void Setup()
        {
            _labelRepository = new Mock<ILabelsRepository>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(h => h.HttpContext.User).Returns(user);
            _labelContract = new LabelsRepository(DBContext, _httpContextAccessor.Object);
            _labelRepository.Setup(p => p.AddLabels(It.IsAny<Labels>())).Returns(Task.FromResult(_label));
            _labelRepository.Setup(p => p.DeleteLabel(It.IsAny<int>())).Returns(Task.FromResult(1));
            _labelRepository.Setup(p => p.GetLabel(It.IsAny<int>())).Returns(Task.FromResult(_label));
        }

        [Test]
        public async Task AddLabelTest()
        {
            Labels result = await _labelContract.AddLabels(new Labels() { LabelName = "testforresult" });
            Assert.IsNotNull(result);
            Assert.AreEqual("testforresult", result.LabelName);
        }

        [Test]
        public async Task DeleteLabelTest()
        {
            Labels label = await _labelContract.GetLabel(2006);
            await _labelContract.DeleteLabel(2006);
            Assert.AreEqual(2006, label.LabelId);
        }
    }
}
