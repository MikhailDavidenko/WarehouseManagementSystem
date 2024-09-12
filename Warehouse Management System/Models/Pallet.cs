using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models
{
    public class Pallet : Item
    {
        private const decimal BaseWeight = 30; // Вес паллеты без коробок
        public List<Box> Boxes { get; }

        public override decimal Weight => BaseWeight + Boxes.Sum(b => b.Weight);

        public Pallet(string id, decimal width, decimal height, decimal depth)
            : base(id, width, height, depth)
        {
            Boxes = new List<Box>();
        }

        public override decimal CalculateVolume()
        {
            return Width * Height * Depth + Boxes.Sum(b => b.CalculateVolume());
        }

        public DateTime? GetEarliestExpirationDate()
        {
            if (Boxes.Count == 0) return null;
            return Boxes.Min(b => b.ExpirationDate);
        }

        public bool CanFitBox(Box box)
        {
            return box.Width <= Width && box.Depth <= Depth;
        }

        public void AddBox(Box box)
        {
            if (CanFitBox(box))
            {
                Boxes.Add(box);
            }
            else
            {
                throw new InvalidOperationException("Box dimensions exceed pallet capacity.");
            }
        }

        protected override void ValidateDimensions()
        {
            if (Width <= 0 || Height <= 0 || Depth <= 0)
                throw new ArgumentException("Pallet dimensions must be greater than zero.");
        }
    }
}
