﻿using System.ComponentModel.DataAnnotations;

namespace Complains_MVCs.Models
{
    public class RegisterReq
    {
       
        [Required,EmailAddress]
        public string Email { get; set; }=string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }= string.Empty;
        
        
    }
}
