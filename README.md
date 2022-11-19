# Rectangles

Approach made is by using Web Api to manipulate board and rectangles

`board.json` is included for better handling and testing of data

- `POST /api/Rectangle` => Generate new Board (supply dimension in request body)
- `PUT /api/Rectangle`  => Place rectangle/s on the board (supply width as int, height as int and coordinate as string comma delimited))
- `GET /api/Rectangle`  => Get current board
- `DELETE /api/Rectangle` => Delete rectangle (supply coordinates as starting point)

Format of coordinates is `(row,column)`

TechStack:
- `.NET 6`
- `C#`
- `NUnit 3.13.3`

Tools:
- `Visual Studio 2022`

Examples:

     5x5 board
        |0,0|0,1|0,2|0,3|0,4|
        |1,0|1,1|1,2|1,3|1,4|
        |2,0|2,1|2,2|2,3|2,4|
        |3,0|3,1|3,2|3,3|3,4|
        |4,0|4,1|4,2|4,3|4,4|


     formula to get coordinates range based on width and height
     row + height - 1  
     column + width - 1

     Example: width = 2; height = 2; coordinates = (0,0)
       coodinates format (row, column)
       row + height:  0 + 2 = 2 - 1
       column + width: 0 + 2 = 2 - 1
       Result:
       | 0,0 | 0,1 |
       | 1,0 | 1,1 |

     Example: width = 3; height = 2; coordinates = (2,2)
       coodinates format (row, column)
       row + height:  2 + 2 = 4 - 1
       column + width: 2 + 3 = 5 - 1
       Result:
       | 2,2 | 2,3 | 2,4 |
       | 3,2 | 3,3 | 3,4 |

     Example: width = 2; height = 3; coordinates = (2,2) 
       coodinates format (row, column)
       row + height:  2 + 3 = 5 - 1
       column + width: 2 + 2 = 4 - 1
       Result:
       | 2,2 | 2,3 |
       | 3,2 | 3,3 |
       | 4,2 | 4,3 |
       
       
 *Unit Test was set to run on specific order due to file manipulation.*
