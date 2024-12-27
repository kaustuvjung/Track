using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Track.Models
{
    public class OrderClass
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Arival Date is required")] 
        public DateTime? Arival{ get; set; }

        [Required]
        public string? Invoice_no { get; set; }
      
        [Required]
        public int vendor_id{ get; set; }

        [ValidateNever]
        [ForeignKey("vendor_id")]
        public VendorClass? vendor { get; set; }

    }
}
