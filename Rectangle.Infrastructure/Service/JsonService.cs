using System;
using System.Text.Json;
using Rectangles.Core.Entity;
using Rectangles.Core.Interface;

namespace Rectangles.Infrastructure.Service
{
	public class JsonService : IJsonService
    {
        private string path = @"..\..\..\..\..\Rectangles\Rectangles.Infrastructure\Data\";
        private string jsonString = File.ReadAllText("board.json");
        public JsonService()
		{
		}

        public async Task<List<Rectangle>> GetRectangles()
        {
            var list = JsonSerializer.Deserialize<List<Rectangle>>(jsonString);
            return list;
        }

        public void UpdateRectangleFile(List<Rectangle> rectangles)
        {
            string json = JsonSerializer.Serialize(rectangles);
            File.WriteAllText("board.json", json);
        }
    }
}

