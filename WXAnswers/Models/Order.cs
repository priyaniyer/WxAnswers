using System;
using System.Collections.Generic;
using System.Text;

namespace WXAnswers.Models
{
    public class Order
    {
        public List<Product> Products { get; set; }
        public List<Special> Specials { get; set; }
        public List<Quantity> Quantities { get; set; }
    }
}
