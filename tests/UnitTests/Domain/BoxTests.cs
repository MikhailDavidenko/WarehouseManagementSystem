using FluentAssertions;
using FluentAssertions.Execution;
using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.UnitTests.Domain;

public sealed class BoxTests
{
    [Fact]
    public void CalculateVolume_BoxWithValidDimensions_ReturnsVolume()
    {
        // Arrange
        var box = Box.Create( 50, 30, 40, 10, Guid.NewGuid(), DateTime.Now);

        // Act
        var result = box.CalculateVolume();

        // Assert
        Assert.Equal(60000, result); // 50 * 30 * 40
    }

    [Fact]
    public void Create_BoxWithValidDimensions_ReturnsBox()
    {
        // Arrange
        double width = 10;
        double height = 20;
        double depth = 30;
        double weight = 40;
        Guid palletId = Guid.NewGuid();
        DateTime productionDate = DateTime.Now.AddDays(-5);


        // Act
        Box box = Box.Create(width, height, depth, weight, palletId, productionDate: productionDate);

        // Assert
        using (new AssertionScope())
        {
            box.Should().NotBeNull();
            box.Width.Should().Be(width);
            box.Height.Should().Be(height);
            box.Depth.Should().Be(depth);
            box.Weight.Should().Be(weight);
            box.PalletId.Should().Be(palletId);
            box.ProductionDate.Should().Be(productionDate);
        }
    }

    [Fact]
    public void Create_BoxWithInvalidDimensions_ThrowsArgumentException()
    {
        // Arrange
        double width = -1;
        double height = 20;
        double depth = 30;
        double weight = 40;
        Guid palletId = Guid.NewGuid();

        // Act
        Action act = () => Box.Create(width, height, depth, weight, palletId, DateTime.Now);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ChangeDimensionsAndWeight_BoxWithValidDimensions_ReturnsBox()
    {
        // Arrange
        Box box = Box.Create(10, 20, 30, 40, Guid.NewGuid(), DateTime.Now);
        double newWidth = 50;
        double newHeight = 60;
        double newDepth = 70;
        double newWeight = 80;

        // Act
        box.ChangeDimensionsAndWeight(newWidth, newHeight, newDepth, newWeight);

        // Assert
        using(new AssertionScope())
        {
            box.Width.Should().Be(newWidth);
            box.Height.Should().Be(newHeight);
            box.Depth.Should().Be(newDepth);
            box.Weight.Should().Be(newWeight);
        }
    }

    [Fact]
    public void ChangeDimensionsAndWeight_BoxWithInvalidDimensions_ThrowsArgumentException()
    {
        // Arrange
        Box box = Box.Create(10, 20, 30, 40, Guid.NewGuid(), DateTime.Now);
        double newWidth = -1;
        double newHeight = 60;
        double newDepth = 70;
        double newWeight = 80;

        // Act
        Action act = () => box.ChangeDimensionsAndWeight(newWidth, newHeight, newDepth, newWeight);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MoveToAnotherPallet_BoxWithValidPalletId_ReturnsBox()
    {
        // Arrange
        Box box = Box.Create(10, 20, 30, 40, Guid.NewGuid(), DateTime.Now);
        var newPallet = Pallet.Create(100, 100, 100);

        // Act
        box.MoveToAnotherPallet(newPallet.Id);

        // Assert
        Assert.Equal(newPallet.Id, box.PalletId);
    }
}
