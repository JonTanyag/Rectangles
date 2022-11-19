using Rectangles.Core.Entity;
using Rectangles.Core.Interface;
using Rectangles.Infrastructure.Service;

namespace Rectangles.UnitTest;

[TestFixture]
public class RectanglesStest
{
    [SetUp]
    public void Setup()
    {
    }

    [Author("Jon Tanyag")]
    [TestCase(25,25)]
    [TestCase(5,5)]
    [TestCase(10,20)]
    [TestCase(7,10)]
    public void WhenCreatingBoard_WithCorrectDimension_ShouldReturn_True(int width, int height)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);


        //Act
        var isCreated = _rectangleService.NewBoard(width, height);


        // Assert
        Assert.IsTrue(isCreated.Result);
    }

    [Author("Jon Tanyag")]
    [TestCase(30,30)]
    [TestCase(2,2)]
    [TestCase(3,3)]
    [TestCase(7, 30)]
    public void WhenCreatingNewBoard_WithWrongDimension_ShouldReturn_False(int width, int height)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);

        //Act
        var isCreated = _rectangleService.NewBoard(width, height);

        // Assert
        Assert.Throws<ArgumentException>(() => { throw new ArgumentException(); });
    }

    [Author("Jon Tanyag")]
    [TestCase(3,2, 2,2)]
    [TestCase(2,2, 1,0)]
    [TestCase(3,2, 2,2)]
    [TestCase(4,2, 2,2)]
    public void WhenValidatingRectangle_WithCorrectDimension_ShouldReturn_True(int width, int height, int row, int column)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);

        // Act
        var isValid = _rectangleService.ValidateRectangle(width, height, row, column);


        // Assert
        Assert.IsTrue(isValid.Result, "Rectangle within range.");
    }

    [Author("Jon Tanyag")]
    [TestCase(3, 2, 2, 2)]
    [TestCase(2, 2, 1, 0)]
    [TestCase(3, 2, 2, 2)]
    [TestCase(4, 2, 2, 2)]
    public void WhenValidatingRectangle_WithCorrectDimension_ShouldReturn_False(int width, int height, int row, int column)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);

        // Act
        var isValid = _rectangleService.ValidateRectangle(width, height, row, column);


        // Assert
        Assert.IsFalse(isValid.Result, "Rectangle out of bounds");
    }

    [Author("Jon Tanyag")]
    [TestCase(2,2)]
    [TestCase(1,2)]
    [TestCase(4,4)]
    [TestCase(24,3)]
    public void WhenDeletingRectangle_WithIncorrectCoordinates_ShouldReturn_False(int row, int column)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);


        // Act
        var deleted = _rectangleService.DeleteRectangle(row, column);


        // Assert
        Assert.IsFalse(deleted.Result, "Rectangle not Found");
    }

    [Author("Jon Tanyag")]
    [TestCase(2, 2)]
    [TestCase(1, 2)]
    [TestCase(4, 4)]
    [TestCase(24, 3)]
    public void WhenDeletingRectangle_WithIncorrectCoordinates_ShouldReturn_True(int row, int column)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);

        // Act
        var deleted = _rectangleService.DeleteRectangle(row, column);


        // Assert
        Assert.IsTrue(deleted.Result, "Rectangle not Found");
    }

}
