using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Track.Models
{
    public class LocalBodyClass
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [ValidateNever]
        [ForeignKey("DistrictId")]
        public DistrictClass? District { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(400)]
        public string? NameNp { get; set; }

        [Required]
        public bool isMunicipality { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public int? IMUCode { get; set; }   

    
    }
}
