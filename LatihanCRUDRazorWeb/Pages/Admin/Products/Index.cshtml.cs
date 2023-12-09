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
	public class IndexModel : PageModel
    {
        private readonly AppDbContext context;

        // Pagination functionality
        public int pageIndex = 1;
        public int totalPage = 0;
        private readonly int pageSize = 5;

        // Search Functionality
        public string search = "";

        // Sort Functionality
        public string column = "Id";
        public string orderBy = "desc";

        public List<Product> Products { get; set; } = new List<Product>();

        public IndexModel(AppDbContext context)
        {
            this.context = context;
        }

        public void OnGet(int? pageIndex, string? search, string? column, string? orderBy)
        {
            IQueryable<Product> query = context.Products;

            // Search Functionality
            if(search != null)
            {
                this.search = search;
                query = query.Where(p => p.Name.Contains(search) || p.Brand.Contains(search));
            }

            // Sort Functionality
            string[] validColumns = { "Id", "Name", "Brand", "Category", "Price", "CreatedAt" };
            string[] validOrderBy = { "desc", "asc" };

            if (!validColumns.Contains(column))
            {
                column = "Id";
            }

            if (!validOrderBy.Contains(orderBy))
            {
                orderBy = "desc";
            }

            this.column = column;
            this.orderBy = orderBy;

            if(column == "Name")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Name);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Name);
                }
            }

            if (column == "Brand")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Brand);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Brand);
                }
            }

            if (column == "Category")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Category);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Category);
                }
            }

            if (column == "Price")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Price);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Price);
                }
            }

            if (column == "CreatedAt")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.CreatedAt);
                }
                else
                {
                    query = query.OrderByDescending(p => p.CreatedAt);
                }
            }
            else
            {
                if(orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Id);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Id);
                }
            }

            // query = query.OrderByDescending(p => p.Id);

            // Pagination functionality
            if (pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }

            this.pageIndex = (int)pageIndex;

            decimal count = query.Count();
            totalPage = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((this.pageIndex - 1) * pageSize)
                .Take(pageSize);

            Products = query.ToList();
        }
    }
}
