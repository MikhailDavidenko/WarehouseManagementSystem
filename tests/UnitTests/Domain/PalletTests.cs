using FluentAssertions;
using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.UnitTests.Domain;

public sealed class PalletTests
{
    [Fact]
    public void AddBox_ShouldAddBoxSuccessfully_WhenBoxFits()
    {
        // Arrange
        var box = Box.Create( 50, 30, 40, 10,Guid.NewGuid(), DateTime.Now);
        var pallet = Pallet.Create( 200, 100, 150);

        // Act
        pallet.AddBox(box);

        // Assert
        pallet.Boxes[0].Should().BeEquivalentTo(box).And.NotBeNull();
    }

    [Fact]
    public void AddBox_ShouldThrowException_WhenBoxDoesNotFit()
    {
        // Arrange
        var box = Box.Create(300, 200, 250, 10, Guid.NewGuid(), DateTime.Now);
        var pallet = Pallet.Create(200, 100, 150);

        // Act
        Action act = () => pallet.AddBox(box);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Weight_ShouldCalculateCorrectly()
    {
        // Arrange
        var box1 = Box.Create(50, 30, 40, 10, Guid.NewGuid(), DateTime.Now);
        var box2 = Box.Create(40, 20, 30, 5, Guid.NewGuid(), DateTime.Now);
        var pallet = Pallet.Create(200, 100, 150);

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Assert
        pallet.Weight.Should().Be(45);
    }

    [Fact]
    public void CalculateVolume_ShouldCalculateCorrectly()
    {
        // Arrange
        var box1 = Box.Create(50, 30, 40, 10, Guid.NewGuid(), DateTime.Now);
        var box2 = Box.Create(40, 20, 30, 5, Guid.NewGuid(), DateTime.Now);
        var pallet = Pallet.Create(200, 100, 150);

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Assert
        pallet.CalculateVolume().Should().Be(3084000);
    }

    [Fact]
    public void GetEarliestExpirationDate_ShouldCalculateCorrectly()
    {
        // Arrange
        var box1 = Box.Create( 50, 30, 40, 10, Guid.NewGuid(), productionDate: DateTime.Today.AddDays(-50));
        var box2 = Box.Create(40, 20, 30, 5, Guid.NewGuid(), expirationDate: DateTime.Today.AddDays(30));
        var pallet = Pallet.Create(200, 100, 150);
        var expectedExpirationDate = DateTime.Today.AddDays(30);

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Assert
        pallet.GetEarliestExpirationDate().Should().Be(expectedExpirationDate);
    }

    [Fact]
    public void CanFitBox_BoxFits_ReturnsTrue()
    {
        // Arrange
        var pallet = Pallet.Create(200, 100, 150);
        var box = Box.Create(50, 50, 50, 10, Guid.NewGuid(), DateTime.Now);

        // Act
        var result = pallet.CanFitBox(box);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanFitBox_BoxDoesNotFit_ReturnsFalse()
    {
        // Arrange
        var pallet = Pallet.Create(200, 100, 150);
        var box = Box.Create(300, 200, 250, 10, Guid.NewGuid(), DateTime.Now);

        // Act
        var result = pallet.CanFitBox(box);

        // Assert
        result.Should().BeFalse();
    }
}
