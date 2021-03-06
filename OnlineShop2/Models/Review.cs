﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu!")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

       
    }
}