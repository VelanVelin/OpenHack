using System;
using System.Collections.Generic;
using System.Text;

namespace CreateRating.Model
{
    class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }
}
