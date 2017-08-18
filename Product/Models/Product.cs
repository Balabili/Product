using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product.Models
{
    public class ProductItem
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public DateTime LastViewDate { get; set; }
    }
}