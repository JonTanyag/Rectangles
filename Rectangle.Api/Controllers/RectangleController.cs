using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rectangles.Core.Entity;
using Rectangles.Core.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rectangles.Api.Controllers
{
    [Route("api/[controller]")]
    public class RectangleController : Controller
    {
        private readonly IRectangleService _rectangeLservice;
        public RectangleController(IRectangleService rectangleService)
        {
            _rectangeLservice = rectangleService;
        }

        // GET: api/values
        [HttpGet]
        public async Task<List<Rectangle>> Get()
        {
            return await _rectangeLservice.GetRectangles();
        }

        // GET api/values/5
        [HttpGet("{row}/{column}")]
        public string GetRectangle(int row, int column)
        {
           var rectangle = _rectangeLservice.GetRectangleByCoordinates(row, column);

            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<bool> Post(int width, int height, [FromBody]string value)
        {
            var isCreated = await _rectangeLservice.NewBoard(width, height);

            return isCreated;
        }

        // PUT api/values/5
        [HttpPut()]
        public async Task<bool> Put(int? id, [FromBody]List<Payload> payload)
        {
            var isUpdated = await _rectangeLservice.PlaceRectangle(payload);
            return isUpdated;
        }

        // DELETE api/values/5
        [HttpDelete()]
        public async void Delete(int row, int column)
        {
            var res = await _rectangeLservice.DeleteRectangle(row, column);
        }
    }
}

