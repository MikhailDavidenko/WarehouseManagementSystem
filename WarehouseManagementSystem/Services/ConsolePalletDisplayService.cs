using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Interfaces;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Services
{
    public class ConsolePalletDisplayService : IPalletDisplayService
    {
        public void DisplayPalletsGroupedByExpiration(List<Pallet> pallets)
        {
            var groupedPallets = pallets
                .OrderBy(p => p.GetEarliestExpirationDate().Value.Date)
                .ThenBy(p => p.Weight);

            Console.WriteLine("Grouped Pallets by Expiration:");
            foreach (var pallet in groupedPallets)
            {
                Console.WriteLine($"Pallet ID: {pallet.Id}, Weight: {pallet.Weight}, Expiration Date: " +
                    $"{pallet.GetEarliestExpirationDate():dd.MM.yyyy}");
            }
        }

        public void DisplayTopThreePalletsWithLongestShelfLife(List<Pallet> pallets)
        {
            var topPallets = pallets
                .OrderByDescending(p => p.GetEarliestExpirationDate().Value.Date)
                .ThenBy(p => p.CalculateVolume())
                .Take(3);

            Console.WriteLine("\nTop 3 Pallets with Longest Shelf Life:");
            foreach (var pallet in topPallets)
            {
                Console.WriteLine($"Pallet ID: {pallet.Id}, Volume: {pallet.CalculateVolume()}, Expiration Date: " +
                    $"{pallet.GetEarliestExpirationDate():dd.MM.yyyy}");
            }
        }
    }
}
