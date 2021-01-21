using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [MinLength(1)]
        [MaxLength(50)]
        [Required(ErrorMessage = "Continutul este obligatoriu!")]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

}