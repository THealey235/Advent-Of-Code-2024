using System.Numerics;
using System.Runtime.InteropServices;

public class P1
{
    private int total = 0;
    private Vector position;
    private char[][] board;
    private int direction = 0;
    private List<Vector> directions;

    private char positionValue
    {
        get
        {
            return board[position.Y][position.X];
        }
        set
        {
            board[position.Y][position.X] = value;
        }
    }

    private Vector directionVector
    {
        get
        {
            return directions[direction];
        }
    }

    public P1(Vector position, char[][] board)
    {
        this.position = position;
        this.board = board;

        directions = new List<Vector>()
        {
            new Vector(0, -1), // 0 = up
            new Vector(1, 0), // 1 = right
            new Vector(0, 1), //3 = down
            new Vector(-1, 0)
        };
    }

    public List<LocationInfo> Run()
    {
        var locationsVisited = new List<LocationInfo>();
        positionValue = 'X';
        total++;
        Vector nextPosition = position;
        while (Vector.InBounds(position + directionVector, board[0].Length - 1, board.Length - 1))
        {
            nextPosition = position + directionVector;
            if (board[nextPosition.Y][nextPosition.X] == '#')
                direction = (direction + 1) % 4;

            position += directionVector;
            if (positionValue == 'X')
                continue;
            positionValue = 'X';
            total++;
            locationsVisited.Add(new LocationInfo(position.X, position.Y, direction));

        }
        Console.WriteLine("Part 1: " + total);
        //does this for part 2
        return locationsVisited;
    }

}

