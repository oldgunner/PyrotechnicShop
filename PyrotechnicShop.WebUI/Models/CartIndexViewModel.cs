using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PyrotechnicShop.Domain.Entities;

namespace PyrotechnicShop.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}