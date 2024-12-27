using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Track.Models
{
    public class CustomerClass
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Range(9800000000,9899999999)]
        public long? PhoneNumber { get; set; }

        public int? ProvinceId { get; set; }
        [ValidateNever]
        [ForeignKey("ProvinceId")]
        public ProvinceClass? Province { get; set; }
        public int? DistrictId { get; set; }

        [ValidateNever]
        [ForeignKey("DistrictId")]
        public DistrictClass? District { get; set; }

        public int? LocalBodyId { get; set; }
        [ValidateNever]
        [ForeignKey("LocalBodyId")]
        public LocalBodyClass? LocalBody { get; set; }
        
        public string? Address { get; set; }    
    }
}
