using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_comp.Models;

namespace project_comp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RegistersController(ApplicationDbContext Context)
        {
            _context = Context;
        }
       
        [HttpPost("Register")]
        public async Task<IActionResult> Register(User request)
        {
            if(_context.Users.Any(u=>u.Email ==request.Email ))
            {
                return BadRequest("User already exists");
            }
            await _context.Users.AddAsync(request);
            
            await _context.SaveChangesAsync();
            
            return Ok(request);
        }

        [HttpPost("Login")]
        public IActionResult Login(RegisterReq request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user != null)
            {
                // Return the user data upon successful login
                return Ok(user);
            }
            else
            {
                return BadRequest("Wrong Email Or Password");
            }
        }
    }
}
