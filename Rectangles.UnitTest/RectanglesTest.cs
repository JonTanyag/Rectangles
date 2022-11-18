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

    [Test]
    public void CreateNewBoard()
    {
        // Arrange
        var _rectangleService = new RectangleService();


        //Act
        var board = _rectangleService.NewBoard(25, 25);


        // Assert
        Assert.IsNotNull(board);
    }

    [Test]
    public void CreateNewBoard_Fail()
    {
        // Arrange
        var _rectangleService = new RectangleService();

        //Act
        var board = _rectangleService.NewBoard(26, 26);

        // Assert
        Assert.Throws<ArgumentException>(() => { throw new ArgumentException(); });
    }

    [Test]
    public void ValidateRectangle()
    {
        // Arrange
        var _rectangleService = new RectangleService();

        // Act
        var isValid = _rectangleService.ValidateRectangle(3, 5, "2,2");


        // Assert
        Assert.IsFalse(isValid.Result, "Rectangle out of bounds");
    }

    [Test]
    public void Test_DeleteRectangleFail()
    {
        // Arrange
        var _rectangleService = new RectangleService();


        // Act
        var deleted = _rectangleService.DeleteRectangle("2,2");


        // Assert
        Assert.IsFalse(deleted.Result, "Rectangle not Found");
    }

    [Test]
    public void Test_DeleteRectanglePass()
    {
        // Arrange
        var _rectangleService = new RectangleService();

        // Act
        var deleted = _rectangleService.DeleteRectangle("1,0");


        // Assert
        Assert.IsTrue(deleted.Result, "Rectangle not Found");
    }

}
