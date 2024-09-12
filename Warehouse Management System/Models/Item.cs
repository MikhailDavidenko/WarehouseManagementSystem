using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models
{
    public abstract class Item
    {
        public string Id { get; }
        public decimal Width { get; }
        public decimal Height { get; }
        public decimal Depth { get; }
        public abstract decimal Weight { get; }

        protected Item(string id, decimal width, decimal height, decimal depth)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;

            ValidateDimensions();
        }

        public abstract decimal CalculateVolume();

        protected virtual void ValidateDimensions()
        {
            if (Width <= 0 || Height <= 0 || Depth <= 0)
                throw new ArgumentException("Dimensions must be greater than zero.");
        }
    }
}
