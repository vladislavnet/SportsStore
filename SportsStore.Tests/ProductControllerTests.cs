using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" },
            }).AsQueryable());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductListViewModel results = controller.List(null, 2)
                .ViewData.Model as ProductListViewModel;

            Product[] prodArray = results.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" },
            }).AsQueryable());
            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            ProductListViewModel results = controller.List(null, 2)
               .ViewData.Model as ProductListViewModel;

            PagingInfo pageInfo = results.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }


        [Fact]
        public void CanFilterProducts()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" },
            }).AsQueryable());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductListViewModel results = controller.List("Cat2", 1)
                .ViewData.Model as ProductListViewModel;

            Product[] prodArray = results.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.True(prodArray[0].Name == "P2" && prodArray[0].Category == "Cat2");
            Assert.True(prodArray[1].Name == "P4" && prodArray[1].Category == "Cat2");
        }
    }
}
