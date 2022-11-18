using Rectangles.Core.Entity;
using Rectangles.Core.Interface;
using Rectangles.Infrastructure.Service;

namespace Rectangles.UnitTest;

public class RectanglesStest
{
    private readonly IRectangleService _rectangleService;
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateNewBoard()
    {
        // Arrange
        var payload = new Payload();
        var board = new IRectangleService;



        //Act




        // Assert
        Assert.Pass();
    }
}
