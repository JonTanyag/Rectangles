using System;
using Rectangles.Core.Entity;
using Rectangles.Core.Interface;

namespace Rectangles.Infrastructure.Service
{
	public class RectangleService : IRectangleService
	{
        private readonly IJsonService _jsonService;

        public RectangleService()
        {

        }
        public RectangleService(IJsonService jsonService)
		{
            _jsonService = jsonService;
		}

        public async Task<bool> DeleteRectangle(int row, int column)
        {
            try
            {
                var board = await _jsonService.GetRectangles();

                var marker = board.FirstOrDefault(x => x.Row == row && x.Column == column && x.isHit);
                if (marker == null)
                    return false;
                
                // get list of items where Mark is equal to Mark value got from line 28
                var rectangle = board.Where(x => x.Mark == marker?.Mark);

                if (marker?.Mark > 0)
                {
                    foreach (var item in board)
                    {
                        foreach (var r in rectangle)
                        {
                            if (item.isHit && item.Mark == r.Mark)
                            {
                                // deletion of rectangle is by resetting the isHit property to back to false
                                // and Mark (id of rectangle) back to zero
                                item.isHit = false;
                                item.Mark = 0;
                            }
                        }

                    }
                }
                _jsonService.UpdateRectangleFile(board);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<List<Rectangle>> GetRectangles()
        {
            var board = await _jsonService.GetRectangles();
            return board;
        }

        public async Task<List<Rectangle>> NewBoard(int width, int height)
        {
            if (width < 5 || width > 25 || height < 5 || height > 25)
                throw new ArgumentException("Dimension of rectangle cannot be less than 5 and greater than 25");

            // create new board dimension can be supplied
            var board = new List<Rectangle>();
            for (int i = 0; i <= height -1; i++)
            {
                for (int j = 0; j <= width-1; j++)
                {
                    board.Add(new Rectangle { Id = 0, Row = i, Column = j, Mark = 0, isHit = false });
                }
            }
            _jsonService.UpdateRectangleFile(board);

            return board;
        }

        public async Task<List<Rectangle>> PlaceRectangle(List<Payload> payload)
        {
            // details of rectangle is placed in a list, this can be beneficial
            // for multiple rectangles

            var overlap = await CheckOverlap(payload);
            if (overlap)
            {
                throw new Exception("Rectangles overlap with each other.");
            }

            // get generated board from POST endpoint
            var board = await _jsonService.GetRectangles();
            var marker = 0;
            foreach (var item in payload)
            {
                var isOverEdge = await ValidateRectangle(item.Width, item.Height, item.Row, item.Column);
                if (!isOverEdge)
                    throw new Exception("Invalid Rectangle. Dimension is out of bounds to the board's dimension.");

                // marker will be the key to find individual rectangles
                marker++;


                var endRow = item.Row + item.Height - 1;
                var endCol = item.Column + item.Width - 1;

                // second loop can determine if 2nd rectangle will overlap with the 2st one
                foreach (var squareToHit in board)
                {
                    // mark suppied row and colum via coordinates e.g (2,2)
                    if (squareToHit.Row == item.Row && squareToHit.Column == item.Column)
                    {
                        if (squareToHit.isHit)
                        {
                            throw new Exception("Current rectangle in process will overlap with previous rectangle.");
                        }
                        squareToHit.Mark = marker;
                        squareToHit.isHit = true;
                    }

                    else
                    {
                        // if current row and column is greater than endRow and endColumn
                        // break the loop; considered as out of bounds
                        if (squareToHit.Row > endRow && squareToHit.Column > endCol)
                            break;

                        // process column in supplied coordinate in initial row
                        if (squareToHit.Row == item.Row && (squareToHit.Column > item.Column && squareToHit.Column <= endCol))
                        {
                            if (squareToHit.isHit)
                            {
                                throw new Exception("Current rectangle in process will overlap with previous rectangle.");
                            }
                            squareToHit.Mark = marker;
                            squareToHit.isHit = true;
                        }
                        // process next row
                        // nested if includes ">" sign to exclude initial coordinates that was already processed
                        // in line 105-106
                        else if (squareToHit.Row > item.Row)
                            if ((squareToHit.Row > item.Row && squareToHit.Row <= endRow) && (squareToHit.Column >= item.Column && squareToHit.Column <= endCol))
                            {
                                if (squareToHit.isHit)
                                {
                                    throw new Exception("Current rectangle in process will overlap with previous rectangle.");
                                }
                                squareToHit.Mark = marker;
                                squareToHit.isHit = true;
                            }
                    }
                }

                // save update board to json file.
                _jsonService.UpdateRectangleFile(board);
            }

            return board;
        }

        private async Task<bool> CheckOverlap(List<Payload> payload)
        {
            // For Constraint:
            // Rectangles must not overlap
            //          (x,y)   (x,y)
            // Example: (2,2) & (3,0)

            // can determine if one of supplied rectangles overlaps with an existing rectangle in the board
            var board = await _jsonService.GetRectangles();
            foreach (var item in payload)
            {

                // this will determine what column and row the rectangle will end
                var endRow = item.Row + item.Height - 1;
                var endCol = item.Column + item.Width - 1;

                var markedCoordinates = board.Any(x => x.Mark > 0);

                if (markedCoordinates)
                {
                    // get existing rectangle
                    // compare coordinates with new one
                    var overlap = board.Any(x => x.Column == item.Column && x.Row == item.Row && x.isHit);
                    if (overlap)
                        return true;
                        //throw new Exception("Rectangle overlaps with existing rectangle.");

                }
            }
            return false;
        }

        public async Task<bool> ValidateRectangle(int width, int height, int row, int column)
        {
            // For constraint:
            // Rectangles must not extend beyond the edge of the grid

            var board = await _jsonService.GetRectangles();
            // get maximum row and column
            // this will determine the dimenstion of board
            var maxRow = board.OrderByDescending(x => x.Row).First();
            var maxColumn = board.OrderByDescending(x => x.Column).First();

            // this will determine what column and row the rectangle will end
            var endRow = row + height - 1;
            var endCol = column + width - 1;

            if (endRow > maxRow.Row || endCol > maxColumn.Column)
                return false;


            return true;
        }

    }
}

