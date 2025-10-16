using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Category  : BaseEntity
    {

        [Required(ErrorMessage = "Category name is required")]
        [MinLength(3, ErrorMessage = "Category name must be at least 3 characters long")]
        [MaxLength(30, ErrorMessage = "Category name cannot exceed 30 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        //[Range(0, 100, ErrorMessage = "Display order must be between 0 and 100")]
        //[Display(Name = "Display Order")]
        //public int DisplayOrder { get; set; }

        // Navigation Property
        public  List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }

}
