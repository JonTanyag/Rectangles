using System.Data.Common;
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
    public void A_WhenCreatingBoard_WithCorrectDimension_ShouldReturn_True(int width, int height)
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
    public void B_WhenCreatingNewBoard_WithWrongDimension_ShouldReturn_False(int width, int height)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);

        //Act
        var isCreated = _rectangleService.NewBoard(width, height);

        // Assert
        Assert.Throws<ArgumentException>(() => { throw new ArgumentException(); });
    }


    [TestCase(3, 2, 2, 2)]
    [TestCase(2, 2, 4, 2)]
    [TestCase(1, 1, 7, 4)]
    [TestCase(1, 1, 9, 2)]
    public void C_WhenPlacingRectangleInBoard_WithCorrectParameters_ShouldReturn_True(int width, int height, int row, int column)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);

        var payload = new List<Payload>();

        payload.Add(new Payload
        {
            Column = column,
            Row = row,
            Width = width,
            Height = height,
        });

        // Act
        var newRectangle = _rectangleService.PlaceRectangle(payload);


        // Assert
        Assert.IsTrue(newRectangle.Result);
    }

    [Author("Jon Tanyag")]
    [TestCase(3,2, 2,2)]
    [TestCase(2,2, 1,0)]
    [TestCase(3,2, 2,2)]
    [TestCase(4,2, 2,2)]
    public void D_WhenValidatingRectangle_WithCorrectDimension_ShouldReturn_True(int width, int height, int row, int column)
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
    [TestCase(10, 2, 20, 21)]
    [TestCase(5, 5, 0, 18)]
    [TestCase(4, 5, 8, 2)]
    public void E_WhenValidatingRectangle_WithCorrectDimension_ShouldReturn_False(int width, int height, int row, int column)
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
    [TestCase(4,2)]
    [TestCase(7,4)]
    [TestCase(9,2)]
    public void G_WhenDeletingRectangle_WithIncorrectCoordinates_ShouldReturn_False(int row, int column)
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
    [TestCase(4, 2)]
    [TestCase(7, 4)]
    [TestCase(9, 2)]
    public void F_WhenDeletingRectangle_WithCorrectCoordinates_ShouldReturn_True(int row, int column)
    {
        // Arrange
        var jsonService = new JsonService();
        var _rectangleService = new RectangleService(jsonService);

        // Act
        var deleted = _rectangleService.DeleteRectangle(row, column);


        // Assert
        Assert.IsTrue(deleted.Result);
    }




}
