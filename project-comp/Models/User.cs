using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace project_comp.Models
{
    public class User
    {
        [Key]
        
        public int Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
        public string TypeOfUser { get; set; } = "User";

        

    }
}
