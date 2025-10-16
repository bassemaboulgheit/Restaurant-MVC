using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Applications.DTos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [MinLength(3, ErrorMessage = "Category name must be at least 3 characters long")]
        [MaxLength(30, ErrorMessage = "Category name cannot exceed 30 characters")]
        [Display(Name = "Category Name")]
        [Remote(action: "VerifyName", controller: "category", AdditionalFields = "Id")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Navigation Property
        public List<ItemsDto> Items { get; set; } = new List<ItemsDto>();
    }
}
