using System;
using Rectangles.Core.Entity;

namespace Rectangles.Core.Interface
{
	public interface IJsonService
	{
        Task<List<Rectangle>> GetRectangles();
        void UpdateRectangleFile(List<Rectangle> rectangles);
    }
}

