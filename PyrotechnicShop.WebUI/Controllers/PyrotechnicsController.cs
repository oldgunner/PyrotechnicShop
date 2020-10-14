using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PyrotechnicShop.Domain.Abstract;
using PyrotechnicShop.Domain.Concrete;
using PyrotechnicShop.Domain.Entities;
using PyrotechnicShop.WebUI.Infrastructure;
using PyrotechnicShop.WebUI.Models;
using PyrotechnicShop.WebUI.Controllers;


namespace PyrotechnicShop.WebUI.Controllers
{
    public class PyrotechnicsController : Controller
    {
        private IPyrotechnicsRepository repository;
        public int pageSize = 4;

        public PyrotechnicsController(IPyrotechnicsRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            PyrotechnicsListViewModel model = new PyrotechnicsListViewModel
            {
                Pyrotechnics = repository.Pyrotechnics
                .Where(p => category == null || p.Category == category)
                .OrderBy(pyrotechnics => pyrotechnics.PyrotechnicsId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Pyrotechnics.Count() :
                    repository.Pyrotechnics.Where(pyrotechnics => pyrotechnics.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
        public FileContentResult GetImage(int pyrotechnicsId)
        {
            Pyrotechnics pyrotechnics = repository.Pyrotechnics.FirstOrDefault(p => p.PyrotechnicsId == pyrotechnicsId);

            if (pyrotechnics != null)
                return File(pyrotechnics.ImageData, pyrotechnics.ImageMimeType);
            else
                return null;
        }
    }
}