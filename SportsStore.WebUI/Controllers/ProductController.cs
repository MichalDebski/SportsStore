using SportsStore.Domain.Abstract;
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
            repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel();

            model.Products = repository.Products
                                       .Where(p => category == null || p.Category == category)
                                       .OrderBy(p => p.ProductID)
                                       .Skip((page - 1) * PageSize)
                                       .Take(PageSize);

            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = category == null ?
                    repository.Products.Count() : 
                    repository.Products.Where(x => x.Category == category).Count()
            };

            model.CurrentCategory = category;

            return View(model);
        }
    }
}