using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PyrotechnicShop.Domain.Entities;

namespace PyrotechnicShop.Domain.Abstract
{
    public interface IPyrotechnicsRepository
    {
        IEnumerable<Pyrotechnics> Pyrotechnics { get; }
        void SavePyrotechnics(Pyrotechnics pyrotechnics);
        void AddToPyrotechnics(Pyrotechnics pyrotechnics);
    }
}
