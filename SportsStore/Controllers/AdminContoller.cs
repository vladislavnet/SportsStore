using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AdminContoller : Controller
    {
        private IProductRepository repository;

        public AdminContoller(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Products);
    }
}
