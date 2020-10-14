using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PyrotechnicShop.Domain.Abstract;
using PyrotechnicShop.Domain.Entities;
using PyrotechnicShop.WebUI.Controllers;
using PyrotechnicShop.WebUI.Models;

namespace PyrotechnicShop.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Pyrotechnics pyro1 = new Pyrotechnics { PyrotechnicsId = 1, Name = "Алмаз" };
            Pyrotechnics pyro2 = new Pyrotechnics { PyrotechnicsId = 2, Name = "Ёжик" };

            Cart cart = new Cart();

            cart.AddItem(pyro1, 1);
            cart.AddItem(pyro2, 1);

            List<CartLine> result = cart.Lines.ToList();

            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Pyrotechnics, pyro1);
            Assert.AreEqual(result[1].Pyrotechnics, pyro2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Pyrotechnics pyro1 = new Pyrotechnics { PyrotechnicsId = 1, Name = "Алмаз" };
            Pyrotechnics pyro2 = new Pyrotechnics { PyrotechnicsId = 2, Name = "Ёжик" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(pyro1, 1);
            cart.AddItem(pyro2, 1);
            cart.AddItem(pyro1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Pyrotechnics.PyrotechnicsId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }
        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Pyrotechnics pyro1 = new Pyrotechnics { PyrotechnicsId = 1, Name = "Алмаз" };
            Pyrotechnics pyro2 = new Pyrotechnics { PyrotechnicsId = 2, Name = "Ёжик" };
            Pyrotechnics pyro3 = new Pyrotechnics { PyrotechnicsId = 3, Name = "Корсар" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(pyro1, 1);
            cart.AddItem(pyro2, 4);
            cart.AddItem(pyro3, 2);
            cart.AddItem(pyro2, 1);

            // Действие
            cart.RemoveLine(pyro2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Pyrotechnics == pyro2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Pyrotechnics pyro1 = new Pyrotechnics { PyrotechnicsId = 1, Name = "Алмаз", Price = 100 };
            Pyrotechnics pyro2 = new Pyrotechnics { PyrotechnicsId = 2, Name = "Ёжик", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(pyro1, 1);
            cart.AddItem(pyro2, 1);
            cart.AddItem(pyro1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }
        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Pyrotechnics pyro1 = new Pyrotechnics { PyrotechnicsId = 1, Name = "Алмаз" };
            Pyrotechnics pyro2 = new Pyrotechnics { PyrotechnicsId = 2, Name = "Ёжик" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(pyro1, 1);
            cart.AddItem(pyro2, 1);
            cart.AddItem(pyro1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        /// <summary>
        /// Проверяем добавление в корзину
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics> {
                new Pyrotechnics {PyrotechnicsId = 1, Name = "Пиротехническое изделие 1", Category = "Категория 1"}, }
                .AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, null);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Pyrotechnics.PyrotechnicsId, 1);
        }

        /// <summary>
        /// После добавления пиротехнического изделия в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Pyrotechnics_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics> {
                new Pyrotechnics {PyrotechnicsId = 1, Name = "Пиротехническое изделие 1", Category = "Категория 1"}, }
                .AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, null);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null, null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Pyrotechnics(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Pyrotechnics(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
