using System;
using Rectangles.Core.Entity;

namespace Rectangles.Core.Interface
{
	public interface IRectangleService
	{

        Task<bool> NewBoard(int width, int height);
        Task<bool> PlaceRectangle(List<Payload> payload);
        Task<List<Rectangle>> GetRectangles();
        Task<bool> DeleteRectangle(int row, int column);
    }
}

