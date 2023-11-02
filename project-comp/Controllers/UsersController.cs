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

            if (Id == 1)
            {
                var complaints = _context.Files.Include(c => c.Demands).ToList();
                return Ok(complaints);

            }
            return BadRequest("User not found.");



        }
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProfile(int Id)
        {
            var User = await _context.Users.FindAsync(Id);
            if (User == null)
            {
                return BadRequest("User not found.");
            }

            return Ok(new { User.UserName, User.Email, User.Phone });
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetUserComplains(int Id)
        {

            var userComplaints = _context.Files
              .Include(c => c.Demands) // Include demands related to complaints
              .Where(c => c.UserId == Id)
              .ToList();

            return Ok(userComplaints);

        }
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetComplaint(int Id)
        {

            var User = await _context.Files.FindAsync(Id);
            if (User == null)
            {
                return BadRequest("User not found.");
            }

            return Ok(User);

        }





    }



}

