using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PyrotechnicShop.Domain.Abstract;
using PyrotechnicShop.Domain.Entities;

namespace PyrotechnicShop.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IPyrotechnicsRepository repository;

        public AdminController(IPyrotechnicsRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Pyrotechnics);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Pyrotechnics pyrotechnics)
        {

            if (ModelState.IsValid)
            {
                repository.AddToPyrotechnics(pyrotechnics);
                repository.SavePyrotechnics(pyrotechnics); 
                return RedirectToAction("Index");
            }
            else
            {
                return View(pyrotechnics);
            }
        }
        public ViewResult Edit(int pyrotechnicsId)
        {
            Pyrotechnics pyrotechnics = repository.Pyrotechnics
                .FirstOrDefault(p => p.PyrotechnicsId == pyrotechnicsId);
            return View(pyrotechnics);
        }

        [HttpPost]
        public ActionResult Edit(Pyrotechnics pyrotechnics, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if(image != null)
                {
                    pyrotechnics.ImageMimeType = image.ContentType;
                    pyrotechnics.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(pyrotechnics.ImageData, 0, image.ContentLength);
                }
                repository.SavePyrotechnics(pyrotechnics);
                TempData["message"] = string.Format("Изменения в пиротехническом изделии \"{0}\" были сохранены", pyrotechnics.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(pyrotechnics);
            }
        }
    }
}