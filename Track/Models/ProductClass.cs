using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Track.Models
{
    public class ProductClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Modal { get; set; }
        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public string? company { get; set; }

        public int? In_stock { get; set; }

        [ValidateNever]
        public string? ImgUrl { get; set; }
    }
}
