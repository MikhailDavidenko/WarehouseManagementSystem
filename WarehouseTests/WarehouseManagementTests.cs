using NUnit.Framework;
using WarehouseManagementSystem;
using WarehouseManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagementSystem.Models;

namespace WarehouseTests
{
    [TestFixture]
    public class WarehouseManagementTests
    {
        

        [Test]
        public void Box_CalculateVolume_ShouldCalculateCorrectly()
        {
            // Arrange
            var box = new Box("Box1", 50, 30, 40, 10);

            // Act
            var result = box.CalculateVolume();

            // Assert
            Assert.AreEqual(60000, result); // 50 * 30 * 40
        }

        

        [Test]
        public void Pallet_AddBox_ShouldAddBoxSuccessfully_WhenBoxFits()
        {
            // Arrange
            var box = new Box("Box1", 50, 30, 40, 10);
            var pallet = new Pallet("Pallet1", 200, 100, 150);

            // Act
            pallet.AddBox(box);

            // Assert
            Assert.AreEqual(1, pallet.Boxes.Count);
            Assert.AreEqual(box, pallet.Boxes[0]);
        }

        [Test]
        public void Pallet_AddBox_ShouldThrowException_WhenBoxDoesNotFit()
        {
            // Arrange
            var box = new Box("Box1", 300, 200, 250, 10);
            var pallet = new Pallet("Pallet1", 200, 100, 150);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pallet.AddBox(box));
        }

        [Test]
        public void Pallet_Weight_ShouldCalculateCorrectly()
        {
            // Arrange
            var box1 = new Box("Box1", 50, 30, 40, 10);
            var box2 = new Box("Box2", 40, 20, 30, 5);
            var pallet = new Pallet("Pallet1", 200, 100, 150);

            // Act
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            // Assert
            Assert.AreEqual(45, pallet.Weight); // 10 + 5 + 30 (базовый вес паллеты)
        }

        [Test]
        public void Pallet_CalculateVolume_ShouldCalculateCorrectly()
        {
            // Arrange
            var box1 = new Box("Box1", 50, 30, 40, 10);
            var box2 = new Box("Box2", 40, 20, 30, 5);
            var pallet = new Pallet("Pallet1", 200, 100, 150);

            // Act
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            // Assert
            Assert.AreEqual(3084000, pallet.CalculateVolume()); // 200 * 100 * 150 +  (объем коробки 1) +  (объем коробки 2)
        }

        [Test]
        public void Pallet_GetEarliestExpirationDate_ShouldCalculateCorrectly()
        {
            // Arrange
            var box1 = new Box("Box1", 50, 30, 40, 10, productionDate: DateTime.Now.AddDays(-50));
            var box2 = new Box("Box2", 40, 20, 30, 5, expirationDate: DateTime.Now.AddDays(30));
            var pallet = new Pallet("Pallet1", 200, 100, 150);

            // Act
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            // Assert
            var expectedExpirationDate = DateTime.Now.AddDays(30).Date;
            var actualExpirationDate = pallet.GetEarliestExpirationDate().Value.Date;
            Assert.AreEqual(expectedExpirationDate, actualExpirationDate);
        }

        // Тесты для класса ConsolePalletDisplayService
        [Test]
        public void ConsolePalletDisplayService_DisplayPalletsGroupedByExpiration_ShouldDisplayCorrectly()
        {
            // Arrange
            var box1 = new Box("Box1", 50, 30, 40, 10, productionDate: DateTime.Now.AddDays(-50));
            var box2 = new Box("Box2", 40, 20, 30, 5, expirationDate: DateTime.Now.AddDays(30));
            var pallet1 = new Pallet("Pallet1", 200, 100, 150);
            pallet1.AddBox(box1);
            pallet1.AddBox(box2);

            var box3 = new Box("Box3", 60, 40, 50, 20, productionDate: DateTime.Now.AddDays(-80));
            var pallet2 = new Pallet("Pallet2", 300, 150, 200);
            pallet2.AddBox(box3);

            var pallets = new List<Pallet> { pallet1, pallet2 };
            var displayService = new ConsolePalletDisplayService();

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            displayService.DisplayPalletsGroupedByExpiration(pallets);

            // Assert
            var expectedOutput = "Grouped Pallets by Expiration:\n" +
                                 "Pallet ID: Pallet2, Weight: 50, Expiration Date: 03.10.2024\n" +
                                 "Pallet ID: Pallet1, Weight: 45, Expiration Date: 13.10.2024\n";

            Assert.AreEqual(expectedOutput.Replace("\r\n", ""), output.ToString().Replace("\r\n", "\n"));

        }

    }
}
