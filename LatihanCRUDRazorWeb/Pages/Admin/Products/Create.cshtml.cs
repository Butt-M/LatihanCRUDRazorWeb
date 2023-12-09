using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatihanCRUDRazorWeb.Models;
using LatihanCRUDRazorWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatihanCRUDRazorWeb.Pages.Admin.Products
{
	public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment environment; // Used to allowed us to save image to server
        private readonly AppDbContext context;

        [BindProperty] // to use property object to the html form
        public ProductDTO ProductDTO { get; set; } = new ProductDTO();

        public CreateModel(IWebHostEnvironment environment, AppDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Imagefile validation
            if (ProductDTO.ImageFile == null)
            {
                ModelState.AddModelError("ProductDTO.ImageFile", "The image file is required");
            }

            // Validation error
            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields";
                return;
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(ProductDTO.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using(var stream = System.IO.File.Create(imageFullPath))
            {
                ProductDTO.ImageFile.CopyTo(stream);
            }


            // save the new product in the database

            Product product = new Product()
            {
                Name = ProductDTO.Name,
                Brand = ProductDTO.Brand,
                Category = ProductDTO.Category,
                Price = ProductDTO.Price,
                Description = ProductDTO.Description ?? "",
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now
            };

            context.Products.Add(product);
            context.SaveChanges();

            // clear the form
            ProductDTO.Name = "";
            ProductDTO.Brand = "";
            ProductDTO.Category = "";
            ProductDTO.Price = 0;
            ProductDTO.Description = "";
            ProductDTO.ImageFile = null;

            ModelState.Clear();

            successMessage = "Product created successfully";

            Response.Redirect("/Admin/Products/Index");
        }
    }
}
