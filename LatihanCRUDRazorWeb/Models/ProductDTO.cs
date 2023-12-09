using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LatihanCRUDRazorWeb.Models
{
	public class ProductDTO
	{
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Brand { get; set; }

        [Required, MaxLength(100)]
        public string Category { get; set; }

        [Required]
        public decimal Price { get; set; }

        // Optional
        public string? Description { get; set; }

        // When we create new product its required, but in edit its optional
        public IFormFile? ImageFile { get; set; }
    }
}

