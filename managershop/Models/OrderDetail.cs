﻿using System.ComponentModel.DataAnnotations.Schema;

namespace managershop.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
