using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        private int PageSize = 4;
        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }
        public ViewResult List(int productPage = 1) 
            => View(repository.Products
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize));
        
    }
}
