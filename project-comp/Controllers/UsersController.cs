using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_comp.Models;
using System.Security.Claims;

namespace project_comp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller
    {


        private readonly ApplicationDbContext _context;
       

        public UsersController(ApplicationDbContext Context)
        {
            _context = Context;
           
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<FileComp>>> Getcomplaints(int Id)
        {

            if (Id == 1 || Id == 3)
            {
                var complaints = _context.Files.ToList();
                return Ok(complaints);
                
            }
            return BadRequest("User not found.");



        }
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProfile(int Id)
        {
            var User = await _context.Users.FindAsync(Id);
            if (User == null )
            {
                return BadRequest("User not found.");
            }
            
            return Ok(new { User.UserName, User.Email,  User.Phone });
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetUserComplains(int Id)
        {
            
            if (Id == null)
            {
                return BadRequest("User not found.");
            }
            var complaints = _context.Files
           .Where(c => c.UserId == Id)
           .ToList();
            return Ok(complaints);
        }

        //[HttpGet("admin/complaints")]
        //[Authorize]
        //public IActionResult GetComplaintsForAdmin()
        //{
        //    // Get the user's unique identifier (e.g., from claims)
        //    var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized(); // Or handle authentication failure as needed
        //    }
        //    if (int.TryParse(userId, out int userIdInt))
        //    {
        //        var user = _context.Users.FirstOrDefault(u => u.Id == userIdInt);

        //        if (user != null && user.TypeOfUser)
        //        {
        //            var complaints = _context.Files.ToList();
        //            return Ok(complaints);
        //        }
        //    }

        //    return Forbid(); // Return a 403 Forbidden status if the user is not an admin
        //}




    }



}

