using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PyrotechnicShop.Domain.Abstract;
using PyrotechnicShop.Domain.Entities;

namespace PyrotechnicShop.Domain.Concrete
{
    public class EFPyrotechnicsRepository : IPyrotechnicsRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Pyrotechnics> Pyrotechnics
        {
            get => context.Pyrotechnics;
        }

        public void AddToPyrotechnics(Pyrotechnics pyrotechnics)
        {
            context.Pyrotechnics.Add(pyrotechnics);
            context.SaveChanges();
        }

        public void SavePyrotechnics(Pyrotechnics pyrotechnics)
        {
            if (pyrotechnics.PyrotechnicsId == 0)
                context.Pyrotechnics.Add(pyrotechnics);
            else
            {
                Pyrotechnics dbEntry = context.Pyrotechnics.Find(pyrotechnics.PyrotechnicsId);
                if (dbEntry != null)
                {
                    dbEntry.Name = pyrotechnics.Name;
                    dbEntry.Description = pyrotechnics.Description;
                    dbEntry.Category = pyrotechnics.Category;
                    dbEntry.Price = pyrotechnics.Price;
                    dbEntry.ImageData = pyrotechnics.ImageData;
                    dbEntry.ImageMimeType = pyrotechnics.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
    }
}
