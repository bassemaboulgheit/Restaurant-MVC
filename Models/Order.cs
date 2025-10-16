using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public enum OrderType
    {
        DineIn,
        Takeout,
        Delivery
    }

    public enum OrderStatus
    {
        Pending,
        Preparing,
        Ready,
        Delivered,
        Cancelled
    }
    public class Order : BaseModels
    {

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
        [DataType(DataType.Currency)]
        public decimal TaxAmount { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Discount Amount")]
        [DataType(DataType.Currency)]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Amount")]
        //[DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }


        [Display(Name = "Estimated Delivery Time")]
        [DataType(DataType.DateTime)]
        public DateTime? EstimatedDeliveryTime { get; set; }

        public  List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();








        // Business Logic Methods
        //public void CalculateTotals()
        //{
        //    Subtotal = 0;
        //    foreach (var item in OrderItems)
        //    {
        //        Subtotal += item.Subtotal;
        //    }

        //    // Apply discounts
        //    DiscountAmount = 0;

        //    // Happy hour discount (20% off from 3-5 PM)
        //    if (OrderDate.Hour >= 15 && OrderDate.Hour < 17)
        //    {
        //        DiscountAmount += Subtotal * 0.20m;
        //    }

        //    // Bulk discount (10% off orders over $100)
        //    if (Subtotal > 100)
        //    {
        //        DiscountAmount += Subtotal * 0.10m;
        //    }

        //    // Calculate tax (8.5%)
        //    TaxAmount = (Subtotal - DiscountAmount) * 0.085m;

        //    // Calculate total
        //    TotalAmount = Subtotal + TaxAmount - DiscountAmount;
        //}

        //public void CalculateEstimatedDeliveryTime()
        //{
        //    if (OrderItems == null || OrderItems.Count == 0)
        //        return;

        //    int maxPrepTime = 0;
        //    foreach (var item in OrderItems)
        //    {
        //        if (item.MenuItem != null && item.MenuItem.PreparationTimeMinutes > maxPrepTime)
        //        {
        //            maxPrepTime = item.MenuItem.PreparationTimeMinutes;
        //        }
        //    }

        //    // Add 30 minutes for delivery
        //    EstimatedDeliveryTime = OrderDate.AddMinutes(maxPrepTime + 30);
        //}

        //public bool CanBeCancelled()
        //{
        //    return OrderStatus != OrderStatus.Ready &&
        //           OrderStatus != OrderStatus.Delivered &&
        //           OrderStatus != OrderStatus.Cancelled;
        //}
    }

}
