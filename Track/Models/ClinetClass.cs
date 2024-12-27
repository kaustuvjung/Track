using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Track.Models
{
    public class ClinetClass: IdentityUser
    {
        
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
