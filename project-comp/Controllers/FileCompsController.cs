
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_comp.Models;
using System;

namespace project_comp.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileCompsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FileCompsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public IActionResult AddComp([FromBody] FileComp file)
        {
            if (file == null)
            {
                return BadRequest("Invalid file");
            }

            var newFile = new FileComp

            {
                Text = file.Text,
                ContentType = file.ContentType,
                Status = file.Status,
                UserId = file.UserId,

            };

            _context.Files.Add(newFile);
            _context.SaveChanges();

            return Ok(newFile);
        }




        [HttpPost("uploadf")]
        public Task test(List<IFormFile> formFiles)
        {

            return Task.CompletedTask;
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
        public async Task<ActionResult<List<FileComp>>> UpdateComp(FileComp requset)
        {
            var DbUser = await _context.Files.FindAsync(requset.Id);
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
