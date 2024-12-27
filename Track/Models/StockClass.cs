using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Track.Models
{
    public class StockClass
    {
        [Key]
        public int Id { get; set; }

        public string? serial_number { get; set; }
        public int? Customer_id { get; set; }
        [ValidateNever]
        [ForeignKey("Customer_id")]
        public CustomerClass? Customer { get; set; }
        [Required]
        [StringLength(1)]
        public string? InStock { get; set; }

        [Required]
        public int Product_id { get; set; }
        [ValidateNever]
        [ForeignKey("Product_id")]
        public ProductClass? Product { get; set; }

        public int? billhasProduct_id { get; set; }
        [ValidateNever]
        [ForeignKey("billhasProduct_id")]
        public BillhasProductClass? BillhasProduct{ get; set; }

        [StringLength(1)]
        public string? isDamaged { get; set; }

        public int? chalanihasProduct_id { get; set; }
        [ValidateNever]
        [ForeignKey("chalanihasProduct_id")]
        public ChalanihasProductClass? ChalanihasProduct { get; set; }

        [Required]
        public int Order_Products_id { get; set; }
        [ValidateNever]
        [ForeignKey("Order_Products_id ")]
        public OrderhasProducts OrderhasProducts { get; set; }

        [StringLength(500)]
        public string? Damaged_why { get; set; } 
    }
}
