using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applications.DTos.ItemDTOs;
using Models;

namespace Applications.DTos.OrderItemsDTOs
{
    public class CreateOrderItemsDto
    {

        [Required(ErrorMessage = "Menu Item ID is required")]
        [Display(Name = "Menu Item ID")]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        [Range(0.01, 10000, ErrorMessage = "Unit Price must be between $0.01 and $10,000")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Subtotal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Menu Item")]
        public ItemsDto MenuItem { get; set; }    ////
    }
}
