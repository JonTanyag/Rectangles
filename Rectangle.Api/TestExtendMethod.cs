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
			var board = rectangleService.GetRectangles();

			var filtered = board.Result.FirstOrDefault(x => x.Row == row && x.Column == column);

			if (filtered?.Mark > 0)
			{
                var rectangle = board.Result.Where(x => x.Mark == filtered.Mark).ToList();

                return rectangle;
            }
			return new List<Rectangle>();
		}
	}
}

