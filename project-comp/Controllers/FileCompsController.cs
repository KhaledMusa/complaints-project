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
        public async Task<IActionResult> Create(FileComp complaint)
        {
            _context.Files.Add(complaint);
            await _context.SaveChangesAsync();

            return Ok(complaint);
        }






        [HttpGet("{Id}")]
        public async Task<ActionResult> GetsingleComp(int Id)
        {

            var User = await _context.Files.Include(c => c.Demands).FirstOrDefaultAsync(f => f.Id == Id);
            if (User == null)
            {
                return BadRequest("file not found.");
            }

            return Ok(User);
        }

        [HttpPut]
        public async Task<ActionResult<FileComp>> UpdateComp(FileComp file)
        {
            // Retrieve the existing FileComp from the database
            var existingFile = await _context.Files.Include(c => c.Demands).FirstOrDefaultAsync(f => f.Id == file.Id);

            if (existingFile == null)
            {
                return NotFound(); // Handle the case where the file doesn't exist
            }

            // Update the properties of the existingFile with the values from the incoming 'file'
            existingFile.Id = file.Id;
            existingFile.Text = file.Text;
            existingFile.ContentType = file.ContentType;
            existingFile.fileName = file.fileName;
            existingFile.Status = file.Status;
            existingFile.UserId = file.UserId;


            for (var i = 0; i < file.Demands.Count; i++)
            {
                existingFile.Demands[i].Description = file.Demands[i].Description;

            }
            await _context.SaveChangesAsync();

            return Ok(existingFile);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> CheckedComp(int Id, string Status)
        {

            var existingFile = await _context.Files.Include(c => c.Demands).FirstOrDefaultAsync(f => f.Id == Id);

            if (existingFile == null)
            {
                return NotFound(); 
            }
            Status = "Accepted";
            existingFile.Status = Status;
            await _context.SaveChangesAsync();

            return Ok(existingFile);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> RejComp(int Id)
        {

            var existingFile = await _context.Files.Include(c => c.Demands).FirstOrDefaultAsync(f => f.Id == Id);

            if (existingFile == null)
            {
                return NotFound();
            }
            
            existingFile.Status = "Rejected";
            await _context.SaveChangesAsync();

            return Ok(existingFile);
        }
    }

}
