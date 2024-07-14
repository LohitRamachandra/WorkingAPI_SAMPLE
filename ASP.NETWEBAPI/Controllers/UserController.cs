using ASP.NETWEBAPI.DTO;
using ASP.NETWEBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(RegisterUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    PasswordHash = userDTO.Password,
                };

                IdentityResult result = await _userManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded)
                {
                    return Ok($"User {user.UserName} created successfully");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string Name)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(Name);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok($"User {Name} deleted successfully");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return NotFound($"User with Name {Name} not found");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string userName, RegisterUserDTO userDTO)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                user.UserName = userDTO.UserName;
                user.PasswordHash = userDTO.Password;
                user.Email = userDTO.Email;

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok($"User {userName} updated successfully");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return NotFound($"User with Name {userName} not found");
        }
    }
}
