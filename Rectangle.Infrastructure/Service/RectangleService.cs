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

        public async Task<bool> DeleteRectangle(string coordinates)
        {
            try
            {
                var axis = coordinates.Split(',');

                var board = await _jsonService.GetRectangles();

                var marker = board.FirstOrDefault(x => x.Row == int.Parse(axis[0]) && x.Column == int.Parse(axis[1]) && x.isHit);
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

            //  |0,0|0,1|0,2|0,3|0,4|
            //  |1,0|1,1|1,2|1,3|1,4|
            //  |2,0|2,1|2,2|2,3|2,4|
            //  |3,0|3,1|3,2|3,3|3,4|
            //  |4,0|4,1|4,2|4,3|4,4|


            // formula to get coordinates range based on width and height
            // row + height - 1  
            // column + width - 1

            // Example: width = 2; height = 2; coordinates = (0,0)
            // coodinates format (row, column)
            // row + height:  0 + 2 = 2 - 1
            // column + width: 0 + 2 = 2 - 1
            // Result:
            // | 0,0 | 0,1 |
            // | 1,0 | 1,1 |

            // Example: width = 3; height = 2; coordinates = (2,2)
            // coodinates format (row, column)
            // row + height:  2 + 2 = 4 - 1
            // column + width: 2 + 3 = 5 - 1
            // Result:
            // | 2,2 | 2,3 | 2,4 |
            // | 3,2 | 3,3 | 3,4 |

            // Example: width = 2; height = 3; coordinates = (2,2) 
            // coodinates format (row, column)
            // row + height:  2 + 3 = 5 - 1
            // column + width: 2 + 2 = 4 - 1
            // Result:
            // | 2,2 | 2,3 |
            // | 3,2 | 3,3 |
            // | 4,2 | 4,3 |
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
                var isOverEdge = await ValidateRectangle(item.Width, item.Height, item.Coordinates);
                if (!isOverEdge)
                    throw new Exception("Invalid Rectangle. Dimension is out of bounds to the board's dimension.");

                // marker will be the key to find individual rectangles
                marker++;

                // coordinates is set to comma separated string to minimize properties in payload
                var coordinates = item.Coordinates.Split(',');

                var endRow = int.Parse(coordinates[0]) + item.Height - 1;
                var endCol = int.Parse(coordinates[1]) + item.Width - 1;

                // second loop can determine if 2nd rectangle will overlap with the 2st one
                foreach (var squareToHit in board)
                {
                    // mark suppied row and colum via coordinates e.g (2,2)
                    if (squareToHit.Row == int.Parse(coordinates[0]) && squareToHit.Column == int.Parse(coordinates[1]))
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
                        if (squareToHit.Row == int.Parse(coordinates[0]) && (squareToHit.Column > int.Parse(coordinates[1]) && squareToHit.Column <= endCol))
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
                        else if (squareToHit.Row > int.Parse(coordinates[0]))
                            if ((squareToHit.Row > int.Parse(coordinates[0]) && squareToHit.Row <= endRow) && (squareToHit.Column >= int.Parse(coordinates[1]) && squareToHit.Column <= endCol))
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
                var coordinate = item.Coordinates.Split(',');

                // this will determine what column and row the rectangle will end
                var endRow = int.Parse(coordinate[0]) + item.Height - 1;
                var endCol = int.Parse(coordinate[1]) + item.Width - 1;

                var markedCoordinates = board.Any(x => x.Mark > 0);

                if (markedCoordinates)
                {
                    // get existing rectangle
                    // compare coordinates with new one
                    var overlap = board.Any(x => x.Column == int.Parse(coordinate[1]) && x.Row == int.Parse(coordinate[0]) && x.isHit);
                    if (overlap)
                        return true;
                        //throw new Exception("Rectangle overlaps with existing rectangle.");

                }
            }
            return false;
        }

        public async Task<bool> ValidateRectangle(int width, int height, string coordinates)
        {
            // For constraint:
            // Rectangles must not extend beyond the edge of the grid

            var board = await _jsonService.GetRectangles();
            var coordinate = coordinates.Split(',');
            // get maximum row and column
            // this will determine the dimenstion of board
            var maxRow = board.OrderByDescending(x => x.Row).First();
            var maxColumn = board.OrderByDescending(x => x.Column).First();

            // this will determine what column and row the rectangle will end
            var endRow = int.Parse(coordinate[0]) + height - 1;
            var endCol = int.Parse(coordinate[1]) + width - 1;

            if (endRow > maxRow.Row || endCol > maxColumn.Column)
                return false;


            return true;
        }

    }
}

