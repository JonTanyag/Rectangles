using System;
using Rectangles.Core.Entity;
using Rectangles.Core.Interface;
using Rectangles.Infrastructure.Service;

namespace Rectangles.Api
{
	public static class TestExtendMethod
	{
		public static List<Rectangle> GetRectangleByCoordinates(this IRectangleService rectangleService, int row, int column)
		{
			var rectangle = rectangleService.GetRectangles();

			var filtered = rectangle.Result.Where(x => x.Row == row && x.Column == column).ToList();

			return filtered;
		}
	}
}

