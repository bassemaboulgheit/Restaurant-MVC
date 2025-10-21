using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.DTos.ItemDTOs
{
    public class CreateItemsDto
    {

        [Required(ErrorMessage = "Item name is required")]
        [MinLength(3, ErrorMessage = "Item name must be at least 3 characters long")]
        [MaxLength(30, ErrorMessage = "Item name cannot exceed 30 characters")]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000.00, ErrorMessage = "Price must be between $1 and $10,000")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "Quantity must be between 0 and 1000")]
        public int Quantity { get; set; }

        [MaxLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Display(Name = "Image URL")]
        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }

    }
}
