using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registerModel.Username, Email = registerModel.Email };
                var result = await userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, true);
                    return StatusCode(StatusCodes.Status200OK, "User created successfully");
                }
                string errorMessage = "";
                foreach(var error in result.Errors)
                {
                    errorMessage += error.Description + "\n";
                }

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error in creating the User : {errorMessage} ");

            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Invalid Credentials");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password,true,false);

                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, "User login success ");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid Credentials");

            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Error in Login");
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return StatusCode(StatusCodes.Status200OK, "User logged out ");
        }

    }
}
