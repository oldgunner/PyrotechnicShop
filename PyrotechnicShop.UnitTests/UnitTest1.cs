using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PyrotechnicShop.Domain.Abstract;
using PyrotechnicShop.Domain.Entities;
using PyrotechnicShop.WebUI.Controllers;
using PyrotechnicShop.WebUI.HtmlHelpers;
using PyrotechnicShop.WebUI.Models;

namespace PyrotechnicShop.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics>
            {
                new Pyrotechnics { PyrotechnicsId = 1, Name = "Пиротехническое изделие 1"},
                new Pyrotechnics { PyrotechnicsId = 2, Name = "Пиротехническое изделие 2"},
                new Pyrotechnics { PyrotechnicsId = 3, Name = "Пиротехническое изделие 3"},
                new Pyrotechnics { PyrotechnicsId = 4, Name = "Пиротехническое изделие 4"},
                new Pyrotechnics { PyrotechnicsId = 5, Name = "Пиротехническое изделие 5"}
            });
            PyrotechnicsController controller = new PyrotechnicsController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            PyrotechnicsListViewModel result = (PyrotechnicsListViewModel)controller.List(null, 2).Model;

            // Утверждение (assert)
            List<Pyrotechnics> pyrotechnics = result.Pyrotechnics.ToList();
            Assert.IsTrue(pyrotechnics.Count == 2);
            Assert.AreEqual(pyrotechnics[0].Name, "Пиротехническое изделие 4");
            Assert.AreEqual(pyrotechnics[1].Name, "Пиротехническое изделие 5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics>
            {
                new Pyrotechnics { PyrotechnicsId = 1, Name = "Пиротехническое изделие 1"},
                new Pyrotechnics { PyrotechnicsId = 2, Name = "Пиротехническое изделие 2"},
                new Pyrotechnics { PyrotechnicsId = 3, Name = "Пиротехническое изделие 3"},
                new Pyrotechnics { PyrotechnicsId = 4, Name = "Пиротехническое изделие 4"},
                new Pyrotechnics { PyrotechnicsId = 5, Name = "Пиротехническое изделие 5"}
            });
            PyrotechnicsController controller = new PyrotechnicsController(mock.Object);
            controller.pageSize = 3;

            // Act
            PyrotechnicsListViewModel result = (PyrotechnicsListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Pirotechnics()
        {
            // Организация (arrange)
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics>
            {
                new Pyrotechnics { PyrotechnicsId = 1, Name = "Пиротехническое изделие 1", Category = "Категория 1"},
                new Pyrotechnics { PyrotechnicsId = 2, Name = "Пиротехническое изделие 2", Category = "Категория 2"},
                new Pyrotechnics { PyrotechnicsId = 3, Name = "Пиротехническое изделие 3", Category = "Категория 1"},
                new Pyrotechnics { PyrotechnicsId = 4, Name = "Пиротехническое изделие 4", Category = "Категория 2"},
                new Pyrotechnics { PyrotechnicsId = 5, Name = "Пиротехническое изделие 5", Category = "Категория 3"}
            });
            PyrotechnicsController controller = new PyrotechnicsController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Pyrotechnics> result = ((PyrotechnicsListViewModel)controller.List("Категория 2", 1).Model)
                .Pyrotechnics.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Пиротехническое изделие 2" && result[0].Category == "Категория 2");
            Assert.IsTrue(result[1].Name == "Пиротехническое изделие 4" && result[1].Category == "Категория 2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics>
            {
                new Pyrotechnics { PyrotechnicsId = 1, Name = "Пиротехническое изделие 1", Category = "Малые батареи"},
                new Pyrotechnics { PyrotechnicsId = 2, Name = "Пиротехническое изделие 2", Category = "Большие батареи"},
                new Pyrotechnics { PyrotechnicsId = 3, Name = "Пиротехническое изделие 3", Category = "Малые батареи"},
                new Pyrotechnics { PyrotechnicsId = 4, Name = "Пиротехническое изделие 4", Category = "Большие батареи"},
                new Pyrotechnics { PyrotechnicsId = 5, Name = "Пиротехническое изделие 5", Category = "Средние батареи"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Большие батареи");
            Assert.AreEqual(results[1], "Малые батареи");
            Assert.AreEqual(results[2], "Средние батареи");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new Pyrotechnics[]
            {
                new Pyrotechnics {PyrotechnicsId = 1, Name = "Пиротехническое изделие 1", Category = "Малые батареи"},
                new Pyrotechnics {PyrotechnicsId = 2, Name = "Пиротехническое изделие 2", Category = "Средние батареи"}
            });

            NavController target = new NavController(mock.Object);

            string categoryToSelect = "Малые батареи";

            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics>
            {
                new Pyrotechnics { PyrotechnicsId = 1, Name = "Пиротехническое изделие 1", Category = "Малые батареи"},
                new Pyrotechnics { PyrotechnicsId = 2, Name = "Пиротехническое изделие 2", Category = "Большие батареи"},
                new Pyrotechnics { PyrotechnicsId = 3, Name = "Пиротехническое изделие 3", Category = "Малые батареи"},
                new Pyrotechnics { PyrotechnicsId = 4, Name = "Пиротехническое изделие 4", Category = "Большие батареи"},
                new Pyrotechnics { PyrotechnicsId = 5, Name = "Пиротехническое изделие 5", Category = "Средние батареи"}
            });

            // Организация - создание контроллера
            PyrotechnicsController controller = new PyrotechnicsController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((PyrotechnicsListViewModel)controller.List("Малые батареи").Model).PagingInfo.TotalItems;
            int res2 = ((PyrotechnicsListViewModel)controller.List("Большие батареи").Model).PagingInfo.TotalItems;
            int res3 = ((PyrotechnicsListViewModel)controller.List("Средние батареи").Model).PagingInfo.TotalItems;
            int resAll = ((PyrotechnicsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
