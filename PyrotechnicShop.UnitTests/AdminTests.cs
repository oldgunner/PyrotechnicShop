using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PyrotechnicShop.Domain.Abstract;
using PyrotechnicShop.Domain.Entities;
using PyrotechnicShop.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PyrotechnicShop.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics>
            {
                new Pyrotechnics { PyrotechnicsId = 1, Name = "Пиротехническое изделие 1"},
                new Pyrotechnics { PyrotechnicsId = 2, Name = "Пиротехническое изделие 2"},
                new Pyrotechnics { PyrotechnicsId = 3, Name = "Пиротехническое изделие 3"},
                new Pyrotechnics { PyrotechnicsId = 4, Name = "Пиротехническое изделие 4"},
                new Pyrotechnics { PyrotechnicsId = 5, Name = "Пиротехническое изделие 5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Pyrotechnics> result = ((IEnumerable<Pyrotechnics>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Пиротехническое изделие 1", result[0].Name);
            Assert.AreEqual("Пиротехническое изделие 2", result[1].Name);
            Assert.AreEqual("Пиротехническое изделие 3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Pyrotechnics()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics>
            {
                new Pyrotechnics { PyrotechnicsId = 1, Name = "Пиротехническое изделие 1"},
                new Pyrotechnics { PyrotechnicsId = 2, Name = "Пиротехническое изделие 2"},
                new Pyrotechnics { PyrotechnicsId = 3, Name = "Пиротехническое изделие 3"},
                new Pyrotechnics { PyrotechnicsId = 4, Name = "Пиротехническое изделие 4"},
                new Pyrotechnics { PyrotechnicsId = 5, Name = "Пиротехническое изделие 5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Pyrotechnics pyrotechnics1 = controller.Edit(1).ViewData.Model as Pyrotechnics;
            Pyrotechnics pyrotechnics2 = controller.Edit(2).ViewData.Model as Pyrotechnics;
            Pyrotechnics pyrotechnics3 = controller.Edit(3).ViewData.Model as Pyrotechnics;

            // Assert
            Assert.AreEqual(1, pyrotechnics1.PyrotechnicsId);
            Assert.AreEqual(2, pyrotechnics2.PyrotechnicsId);
            Assert.AreEqual(3, pyrotechnics3.PyrotechnicsId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Pyrotechnics()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics>
            {
                new Pyrotechnics { PyrotechnicsId = 1, Name = "Пиротехническое изделие 1"},
                new Pyrotechnics { PyrotechnicsId = 2, Name = "Пиротехническое изделие 2"},
                new Pyrotechnics { PyrotechnicsId = 3, Name = "Пиротехническое изделие 3"},
                new Pyrotechnics { PyrotechnicsId = 4, Name = "Пиротехническое изделие 4"},
                new Pyrotechnics { PyrotechnicsId = 5, Name = "Пиротехническое изделие 5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Pyrotechnics result = controller.Edit(6).ViewData.Model as Pyrotechnics;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Pyrotechnics pyrotechnics = new Pyrotechnics { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(pyrotechnics);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SavePyrotechnics(pyrotechnics));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Pyrotechnics pyrotechnics = new Pyrotechnics { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(pyrotechnics);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SavePyrotechnics(It.IsAny<Pyrotechnics>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
