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
    public class EditModel : PageModel
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        [BindProperty]
        public ProductDTO ProductDTO { get; set; } = new ProductDTO();

        public Product Product { get; set; } = new Product();

        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            var product = context.Products.Find(id);
            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            ProductDTO.Name = product.Name;
            ProductDTO.Brand = product.Brand;
            ProductDTO.Category = product.Category;
            ProductDTO.Price = product.Price;
            ProductDTO.Description = product.Description;

            Product = product;
        }

        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields";
                return;
            }

            var product = context.Products.Find(id);
            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }



            // update the image file if we have a new image
            string newFileName = product.ImageFileName;
            if(ProductDTO.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(ProductDTO.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    ProductDTO.ImageFile.CopyTo(stream);
                }

                // Delete old image
                string oldImagePath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImagePath);
            }

            // update the product in the database
            product.Name = ProductDTO.Name;
            product.Brand = ProductDTO.Brand;
            product.Category = ProductDTO.Category;
            product.Price = ProductDTO.Price;
            product.Description = ProductDTO.Description ?? "";
            product.ImageFileName = newFileName;

            context.SaveChanges();


            Product = product;
            successMessage = "Product updated successfully";
            Response.Redirect("/Admin/Products/Index");
        }
    }
}
