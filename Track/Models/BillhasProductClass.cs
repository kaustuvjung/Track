using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Track.Models
{
    public class BillhasProductClass
    {
        public int Id { get; set; }
        public int? Bill_id { get; set; }

        [ForeignKey(nameof(Bill_id))]
        [ValidateNever]
        public BillClass? Bill { get; set; }
        public decimal? Rate { get; set; }
        [Required]
        public int Quantity { get; set; }

        public decimal? total { get; set; }
        public int? product_id { get; set; }
        [ForeignKey("product_id ")]
        [ValidateNever]
        public ProductClass? Product { get; set; }

        [StringLength(450)]
        public string? User_id { get; set; }

        public string? Extra_items{ get; set; }
    }
}
