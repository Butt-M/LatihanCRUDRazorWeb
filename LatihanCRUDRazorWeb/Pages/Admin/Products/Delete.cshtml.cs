using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatihanCRUDRazorWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatihanCRUDRazorWeb.Pages.Admin.Products
{
	public class DeleteModel : PageModel
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public DeleteModel(AppDbContext context, IWebHostEnvironment environment)
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

            // Delete Product Image
            string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges();

            Response.Redirect("/Admin/Products/Index");
        }
    }
}
