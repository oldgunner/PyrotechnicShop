using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PyrotechnicShop.Domain.Abstract;
using PyrotechnicShop.Domain.Entities;
using PyrotechnicShop.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PyrotechnicShop.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Pyrotechnics pyrotechnics = new Pyrotechnics
            {
                PyrotechnicsId = 2,
                Name = "Пиротехническое изделие 2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics> {
                new Pyrotechnics {PyrotechnicsId = 1, Name = "Пиротехническое изделие 1"},
                pyrotechnics,
                new Pyrotechnics {PyrotechnicsId = 3, Name = "Пиротехническое изделие 3"}
            }.AsQueryable());

            // Организация - создание контроллера
            PyrotechnicsController controller = new PyrotechnicsController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(pyrotechnics.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IPyrotechnicsRepository> mock = new Mock<IPyrotechnicsRepository>();
            mock.Setup(m => m.Pyrotechnics).Returns(new List<Pyrotechnics> {
                new Pyrotechnics {PyrotechnicsId = 1, Name = "Пиротехническое изделие 1"},
                new Pyrotechnics {PyrotechnicsId = 2, Name = "Пиротехническое изделие 2"}
            }.AsQueryable());

            // Организация - создание контроллера
            PyrotechnicsController controller = new PyrotechnicsController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}
