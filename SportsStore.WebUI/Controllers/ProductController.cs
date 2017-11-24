﻿using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository repository;
        public int PageSize = 4;

        public ProductController(IProductsRepository productRepository)
        {
            this.repository = productRepository;
        }

        public ViewResult List(int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel();

            model.Products = repository.Products
                                       .OrderBy(p => p.ProductID)
                                       .Skip((page - 1) * PageSize)
                                       .Take(PageSize);

            model.PagingInfo = new PagingInfo { CurrentPage = page,
            ItemsPerPage = PageSize,TotalItems =repository.Products.Count()};
                                       

            return View(model);
        }
    }
}