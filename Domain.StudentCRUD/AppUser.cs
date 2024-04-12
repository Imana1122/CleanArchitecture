using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.StudentCRUD
{
    public class AppUser:IdentityUser
    {
        
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string? Phone { get; set; }
    }
}
