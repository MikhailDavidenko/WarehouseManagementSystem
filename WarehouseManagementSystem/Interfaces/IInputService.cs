using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Interfaces
{
    internal interface IInputService
    {
        List<Pallet> GetPallets();
    }
}
