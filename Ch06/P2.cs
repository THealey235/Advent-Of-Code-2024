public class P2
{
    //Yes, i did not follow the standard naming convention. Yes, i regret it
    private int total = 0;
    private Vector startPosition;
    private Vector position;
    private char[][] originalBoard;
    private char[][] currentBoard;
    private int direction;
    private List<Vector> directions;

    //stores which hashtags have been hit and what direction we were facing at that time
    //So, if we are at the same spot with the same dir we know that we have looped
    private List<LocationInfo> record; 

    private char positionValue
    {
        get
        {
            return currentBoard[position.Y][position.X];
        }
        set
        {
            currentBoard[position.Y][position.X] = value;
        }
    }

    private Vector directionVector
    {
        get
        {
            return directions[direction];
        }
    }

    public P2(Vector position, char[][] board)
    {
        this.position = this.startPosition = position;
        this.originalBoard = board;
        record = new List<LocationInfo>();

        directions = new List<Vector>()
        {
            new Vector(0, -1), // 0 = up
            new Vector(1, 0), // 1 = right
            new Vector(0, 1), //3 = down
            new Vector(-1, 0)
        };
    }

    public void Run()
    {
        for (var i = 0; i < originalBoard.Length; i++)
        {
            for (int j = 0; j < originalBoard[0].Length; j++)
            {
                currentBoard = Program.CopyJaggedArray(originalBoard);
                record.Clear();

                if (originalBoard[i][j] != '.')
                    continue;

                positionValue = 'X';
                currentBoard[i][j] = '#';
                position = this.startPosition;
                direction = 0;
                var nextPosition = position;

                while (Vector.InBounds(position + directionVector, currentBoard[0].Length - 1, currentBoard.Length - 1))
                {
                    nextPosition = position + directionVector;
                    if (currentBoard[nextPosition.Y][nextPosition.X] == '#')
                    {
                        direction = (direction + 1) % 4;
                        var newRecord = new LocationInfo(nextPosition.X, nextPosition.Y, direction);
                        if (record.Contains(newRecord))
                        {
                            total++;
                            break;
                        }
                        else
                        {
                            record.Add(newRecord);
                        }
                    }
                    position += directionVector;
                }
            }
        }

        Console.WriteLine("Part 2: " + total);
    }
}