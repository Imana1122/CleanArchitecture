using Domain.StudentCRUD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.StudentCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,     // Assign the Name property
                Gender = model.Gender, // Assign the Gender property
                Phone = model.Phone    // Assign the Phone property
            };

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role!);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                return BadRequest("Invalid role specified.");
            }

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (result.Succeeded)
            {
                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role!);



                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }



        [HttpGet("getAll"),Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users=await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with given ID is not found
            }

            return Ok(user);
        }


        [HttpPut("updateByEmail")]
        public async Task<IActionResult> Update(string email, UpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Name = model.Name;     // Update the Name property
            user.Gender = model.Gender; // Update the Gender property
            user.Phone = model.Phone;   // Update the Phone property

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role!);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                return BadRequest("Invalid role specified.");
            }

            // Get the roles assigned to the user
            var roles = await _userManager.GetRolesAsync(user);
            // Remove existing roles
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            // Assign the specified role to the user
            await _userManager.AddToRoleAsync(user, model.Role!);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok("User updated successfully.");
            }

            return BadRequest(result.Errors);
        }


        [HttpDelete("deleteByEmail")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully.");
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("changepassword"), Authorize]
        
        public async Task<IActionResult> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with given ID is not found
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors); // Return any validation errors if password change fails
            }

            return Ok(result); // Return 204 No Content if password change is successful
        }



    }
}
