using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class OrderItem : BaseEntity
    {
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

        // Navigation Properties
        [Required]
        [Display(Name = "Order")]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public  Order Order { get; set; } 

        [Required]
        [Display(Name = "Menu Item")]
        public int MenuItemId { get; set; }
        [ForeignKey(nameof(MenuItemId))]
        public  MenuItem MenuItem { get; set; } 

    }

}
