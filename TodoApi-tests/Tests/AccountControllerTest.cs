using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using ToDoApi.Database.Models;

namespace TodoApi_tests.Tests
{
    class AccountControllerTest
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private AccountController _accountController;
        private readonly RegisterModel register = new RegisterModel { Email = "test@example.com", Username = "testuser", Password = "Hello@123" };

        [SetUp]
        public void setup()
        {
        //    _userManager = new UserManager<IdentityUser>();
        //    _signInManager = new SignInManager<IdentityUser>();
            _accountController = new AccountController(_userManager, _signInManager);
        }

        [Test]
        public async Task LoginTest()
        {
            IActionResult result = await _accountController.Login(new LoginModel { Username = "testuser", Password = "Hello@123" });
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }

        [Test]
        public async Task RegisterTest()
        {
            IActionResult result = await _accountController.Register(register);
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }

    }
}
