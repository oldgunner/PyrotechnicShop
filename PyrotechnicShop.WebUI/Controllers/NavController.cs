using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PyrotechnicShop.Domain.Abstract;

namespace PyrotechnicShop.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IPyrotechnicsRepository repository;
        public NavController(IPyrotechnicsRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null)
            //,bool horizontalNav = false
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Pyrotechnics
                .Select(pyrotechnics => pyrotechnics.Category)
                .Distinct()
                .OrderBy(x => x);

            //string viewName = horizontalNav ? "MenuHorizontal" : "Menu";
            return PartialView("FlexMenu", categories);
        }
    }
}