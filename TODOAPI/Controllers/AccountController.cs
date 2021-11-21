using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoApi.Database.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        
        public AccountController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registerModel.Username, Email = registerModel.Email };
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
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


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password,true,false);
                
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, "User login success ");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Credentials");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error in Login");
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return StatusCode(StatusCodes.Status200OK, "User logged out ");
        }
    }
}
