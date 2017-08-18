using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Product.Models
{
    [Table("Product")]
    public class ProductModel
    {
        [Key]
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }
    }
}