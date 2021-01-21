using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Required]
        public string UserId { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}