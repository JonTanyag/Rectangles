using System;
using Rectangles.Core.Entity;

namespace Rectangles.Core.Interface
{
	public interface IRectangleService
	{

        Task<List<Rectangle>> NewBoard(int width, int height);
        Task<List<Rectangle>> PlaceRectangle(List<Payload> payload);
        Task<List<Rectangle>> GetRectangles();
        Task<bool> DeleteRectangle(string coordinates);
    }
}

