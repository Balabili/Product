using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Product.Models
{
    [Table("Map")]
    public class Map
    {
        [Key]
        public Guid Id { get; set; }

        public long LastViewDate { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public virtual ProductModel Product { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public virtual UserModel User { get; set; }
    }
}