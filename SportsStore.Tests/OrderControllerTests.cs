using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void CannotCheckoutEmptyCart()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController controller = new OrderController(mock.Object, cart);

            ViewResult result = controller.Checkout(order) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CannotCheckoutInvalidShippingDetails()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController controller = new OrderController(mock.Object, cart);
            controller.ModelState.AddModelError("error", "error");

            ViewResult result = controller.Checkout(new Order()) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CannotCheckoutAndSubmitOrder()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController controller = new OrderController(mock.Object, cart);

            RedirectToActionResult result = controller.Checkout(new Order()) as RedirectToActionResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
