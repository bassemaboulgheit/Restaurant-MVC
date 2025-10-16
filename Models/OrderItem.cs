using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class OrderItem : BaseModels
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

        //[StringLength(500, ErrorMessage = "Special instructions cannot exceed 500 characters")]
        //[Display(Name = "Special Instructions")]
        //[DataType(DataType.MultilineText)]
        //public string SpecialInstructions { get; set; }   

        // Navigation Properties
        [Required]
        [Display(Name = "Order")]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Menu Item")]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }
        public  Order Order { get; set; } 
        public  MenuItem MenuItem { get; set; } = new MenuItem();



        // Business Logic Method
        //public void CalculateSubtotal()
        //{
        //    Subtotal = Quantity * UnitPrice;
        //}
    }

}
