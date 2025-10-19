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
    public class OrderItemsDto
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; } 

        [Required]
        [Display(Name = "Order")]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Menu Item")]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }
        public Order? Order { get; set; }
        public MenuItem? MenuItem { get; set; }
    }
}
