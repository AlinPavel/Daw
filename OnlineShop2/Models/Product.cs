using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Models
{
    public class Product
    {
            [Key]
            public int ProductId { get; set; }

            [Required(ErrorMessage = "Continutul este obligatoriu!")]
            [MaxLength(100)]
            public string Name { get; set; }

            [Required(ErrorMessage = "Continutul este obligatoriu!")]
            [DataType(DataType.MultilineText)]
            public string Description { get; set; }
            [Required(ErrorMessage = "Continutul este obligatoriu!")]
            public int Price { get; set; }
            public DateTime Date { get; set; }

            public string UserId { get; set; }
            public virtual ApplicationUser User { get; set; }
            public virtual ICollection<Category> Categories { get; set; }
            public virtual ICollection<Review> Reviews { get; set; }
            [Required(ErrorMessage = "Selectati cel putin o categorie")]
            public int[] SelectedCategories { get; set; } 

            public virtual ICollection<ApplicationUser> Buyers { get; set; }
            public IEnumerable<SelectListItem> Cat { get; set; } 

    }
}