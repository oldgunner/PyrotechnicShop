using PyrotechnicShop.Domain.Abstract;
using PyrotechnicShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PyrotechnicShop.WebUI.Models;

namespace PyrotechnicShop.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IPyrotechnicsRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IPyrotechnicsRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(Cart cart, int pyrotechnicsId, string returnUrl)
        {
            Pyrotechnics pyrotechnics = repository.Pyrotechnics
                .FirstOrDefault(g => g.PyrotechnicsId == pyrotechnicsId);

            if (pyrotechnics != null)
            {
                cart.AddItem(pyrotechnics, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int pyrotechnicsId, string returnUrl)
        {
            Pyrotechnics pyrotechnics = repository.Pyrotechnics
                .FirstOrDefault(g => g.PyrotechnicsId == pyrotechnicsId);

            if (pyrotechnics != null)
            {
                cart.RemoveLine(pyrotechnics);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        [HttpGet]
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Извините, Ваша корзина пуста");
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
                return View(shippingDetails);
        }
    }
}