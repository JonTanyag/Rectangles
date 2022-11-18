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
