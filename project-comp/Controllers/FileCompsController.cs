
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_comp.Models;
using System;

namespace project_comp.Controllers
{
    [ApiController]
    [Route("api/files/[action]")]
    public class FileCompsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FileCompsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]

        public IActionResult Create(FileComp comp)
        {
            _context.Files.Add(comp);
            _context.SaveChanges();

            return Ok(comp);
        }




       

        [HttpGet("{Id}")]
        public async Task<ActionResult> StatusComp(int Id)
        {
            //var User = await _context.Users.FindAsync(Id);
            //if (User == null)
            //{
            //    return BadRequest("User not found.");
            //}
            //var Statu = await _context.Files.FindAsync(Id);
            //if (Statu.Status == "accepted")
            //{
            //    return Ok("Complain was accepted");
            //}
            //if (Statu.Status == "holding")
            //{
            //    return Ok("Waiting for Response");
            //}
            //return Ok(Statu);
            var User = await _context.Files.FindAsync(Id);
            if (User == null)
            {
                return BadRequest("file not found.");
            }

            return Ok(new { User.Text, User.Status, User.ContentType });
        }

        [HttpPut]
        public async Task<ActionResult<List<FileComp>>> UpdateComp(int Id,FileComp requset)
        {
            var DbUser = await _context.Files.FindAsync(Id);
            if (DbUser == null)
            {
                return BadRequest("User not found.");
            }
             DbUser.Text = requset.Text;
            DbUser.ContentType = requset.ContentType;
            
            
            await _context.SaveChangesAsync();
            return Ok(DbUser);
        }
    }

}
