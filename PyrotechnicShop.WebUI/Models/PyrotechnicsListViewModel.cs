using PyrotechnicShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PyrotechnicShop.WebUI.Models
{
    public class PyrotechnicsListViewModel
    {
        public IEnumerable<Pyrotechnics> Pyrotechnics { get; set; }
        public PagingInfo PagingInfo { get; set; }

       public string CurrentCategory { get; set; }
    }
}