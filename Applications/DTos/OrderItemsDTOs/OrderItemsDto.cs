using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.DTos.ItemDTOs;
using Models;

namespace Applications.DTos.OrderItemsDTOs
{
    public class OrderItemsDto
    {
        [Display(Name = "Order Item ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Menu Item ID is required")]
        [Display(Name = "Menu Item ID")]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Subtotal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        public ItemsDto? MenuItem { get; set; }
    }
}
