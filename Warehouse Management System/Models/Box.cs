using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models
{
    public class Box : Item
    {
        public override decimal Weight { get; }
        public DateTime? ExpirationDate { get; private set; }
        public DateTime? ProductionDate { get; private set; }

        public Box(string id, decimal width, decimal height, decimal depth, decimal weight, DateTime? expirationDate = null, DateTime? productionDate = null)
            : base(id, width, height, depth)
        {
            Weight = weight;
            ProductionDate = productionDate;
            ExpirationDate = expirationDate ?? productionDate?.AddDays(100);
        }

        public override decimal CalculateVolume()
        {
            return Width * Height * Depth;
        }
    }
}
