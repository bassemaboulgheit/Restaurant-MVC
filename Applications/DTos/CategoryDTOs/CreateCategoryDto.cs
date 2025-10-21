using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Applications.DTos.CategoryDTOs
{
    public class CreateCategoryDto
    {

        [Required(ErrorMessage = "Category name is required")]
        [MinLength(3, ErrorMessage = "Category name must be at least 3 characters long")]
        [MaxLength(30, ErrorMessage = "Category name cannot exceed 30 characters")]
        [Display(Name = "Category Name")]
        [Remote(action: "VerifyName", controller: "category", AdditionalFields = "Id")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
