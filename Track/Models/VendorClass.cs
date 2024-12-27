using System.ComponentModel.DataAnnotations;

namespace Track.Models
{
    public class VendorClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string? Description { get; set; }

        [Required]
        [Range(9800000000, 9899999999)]
        public long PhoneNumber { get; set; }   
    }
}
