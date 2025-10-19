using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Applications.DTos
{
    public class OrderDto
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Order type is required")]
        [Display(Name = "Order Type")]
        public OrderType OrderType { get; set; }


        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;


        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;


        [MaxLength(500, ErrorMessage = "Delivery address cannot exceed 500 characters")]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tax Amount (8.5%)")]
        public decimal TaxAmount { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Discount Amount")]
        [DataType(DataType.Currency)]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }


        [Display(Name = "Estimated Delivery Time")]
        [DataType(DataType.DateTime)]
        public DateTime? EstimatedDeliveryTime { get; set; }

        public int TotalPreparationTime { get; set; }

        [ForeignKey("user")]
        public string? userId { get; set; }
        public List<OrderItemsDto>? OrderItems { get; set; } 
        public LoginDto? user { get; set; }
    }
}
