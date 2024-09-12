using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Interfaces;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Services
{
    public class ConsoleInputService : IInputService
    {
        private List<Pallet> _pallets;

        public ConsoleInputService()
        {
            _pallets = new List<Pallet>();
        }

        public List<Pallet> GetPallets()
        {
            char input;
            while (true)
            {
                Console.WriteLine("Choose input method: 1. Generate 2. User Input");
                input = Console.ReadKey().KeyChar;

                if (input == '1')
                {
                    _pallets = GenerateSampleData();
                    break;
                }
                else if (input == '2')
                {
                    _pallets = GetUserInputData();
                    break;
                }
                else
                {
                    Console.WriteLine("Введите 1 или 2");
                }
            }
            

            return _pallets;
        }

        private List<Pallet> GenerateSampleData()
        {
            var pallets = new List<Pallet>();

            // Создаем коробки с различными свойствами
            var box1 = new Box("Box1", 50, 30, 40, 10, productionDate: DateTime.Now.AddDays(-50));
            var box2 = new Box("Box2", 40, 20, 30, 5, expirationDate: DateTime.Now.AddDays(30));
            var box3 = new Box("Box3", 60, 40, 50, 20, productionDate: DateTime.Now.AddDays(-80));
            var box4 = new Box("Box4", 35, 25, 35, 15, productionDate: DateTime.Now.AddDays(-120));
            var box5 = new Box("Box5", 45, 30, 45, 12, expirationDate: DateTime.Now.AddDays(45));
            var box6 = new Box("Box6", 55, 40, 55, 25, productionDate: DateTime.Now.AddDays(-90));

            // Создаем паллеты и добавляем коробки в них
            var pallet1 = new Pallet("Pallet1", 200, 100, 150);
            pallet1.AddBox(box1);
            pallet1.AddBox(box2);

            var pallet2 = new Pallet("Pallet2", 300, 150, 200);
            pallet2.AddBox(box3);

            var pallet3 = new Pallet("Pallet3", 250, 120, 180);
            pallet3.AddBox(box4);
            pallet3.AddBox(box5);

            var pallet4 = new Pallet("Pallet4", 350, 180, 250);
            pallet4.AddBox(box6);

            pallets.Add(pallet1);
            pallets.Add(pallet2);
            pallets.Add(pallet3);
            pallets.Add(pallet4);

            return pallets;
        }

        private List<Pallet> GetUserInputData()
        {
            var pallets = new List<Pallet>();

            while (true)
            {
                Console.Write("Enter pallet ID (or 'q' to quit): ");
                string id = Console.ReadLine();

                if (id.ToLower() == "q")
                    break;

                Console.Write("Enter pallet width: ");
                decimal width = decimal.Parse(Console.ReadLine());

                Console.Write("Enter pallet height: ");
                decimal height = decimal.Parse(Console.ReadLine());

                Console.Write("Enter pallet depth: ");
                decimal depth = decimal.Parse(Console.ReadLine());

                var pallet = new Pallet(id, width, height, depth);

                while (true)
                {
                    Console.Write("Enter box ID (or 'done' to finish pallet): ");
                    string boxId = Console.ReadLine();

                    if (boxId.ToLower() == "done")
                        break;

                    Console.Write("Enter box width: ");
                    decimal boxWidth = decimal.Parse(Console.ReadLine());

                    Console.Write("Enter box height: ");
                    decimal boxHeight = decimal.Parse(Console.ReadLine());

                    Console.Write("Enter box depth: ");
                    decimal boxDepth = decimal.Parse(Console.ReadLine());

                    Console.Write("Enter box weight: ");
                    decimal boxWeight = decimal.Parse(Console.ReadLine());

                    DateTime? expirationDate = null;
                    DateTime? productionDate = null;

                    Console.Write("Enter box expiration date (or 'none' for production date): ");
                    string dateInput = Console.ReadLine();

                    if (dateInput.ToLower() != "none" || string.IsNullOrEmpty(dateInput))
                    {
                        expirationDate = DateTime.Parse(dateInput);
                    }
                    else
                    {
                        dateInput = Console.ReadLine();
                        productionDate = DateTime.Parse(dateInput);
                        
                    }

                    var box = new Box(boxId, boxWidth, boxHeight, boxDepth, boxWeight, expirationDate, productionDate);

                    try
                    {
                        pallet.AddBox(box);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                pallets.Add(pallet);
            }

            return pallets;
        }
    }
}
