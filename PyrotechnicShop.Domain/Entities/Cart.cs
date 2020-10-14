using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyrotechnicShop.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Pyrotechnics pyrotechnics, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Pyrotechnics.PyrotechnicsId == pyrotechnics.PyrotechnicsId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Pyrotechnics = pyrotechnics,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Pyrotechnics pyrotechnics)
        {
            lineCollection.RemoveAll(
                p => p.Pyrotechnics.PyrotechnicsId == pyrotechnics.PyrotechnicsId);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(p => p.Pyrotechnics.Price * p.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get => lineCollection;
        }
    }

    public class CartLine
    {
        public Pyrotechnics Pyrotechnics { get; set; }
        public int Quantity { get; set; }
    }
}
