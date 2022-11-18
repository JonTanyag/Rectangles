using System;
namespace Rectangles.Core.Entity
{
	public class Rectangle
	{
		public Rectangle()
		{
		}

        public int Id { get; set; }
        public int Row { get; set; }
		public int Column { get; set; }
		public int Mark { get; set; }
		public bool isHit { get; set; }
	}
}

