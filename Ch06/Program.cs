using System.IO.Compression;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main(string[] args)
    {
        List<string> content;
        using (var reader = new System.IO.StreamReader("input.txt"))
        {
            content = reader.ReadToEnd().Split("\r\n").ToList();
        }


        //Find the guard's starting point
        Vector position = new Vector(0, 0);
        for (int i = 0; i < content.Count; i++)
        {
            if (content[i].Contains('^'))
            {
                position.Y = i;
                break;
            }
        }
        position.X = content[position.Y].IndexOf('^');

        var board = content.Select(x => x.ToArray()).ToArray();
            
        new P1(position, CopyJaggedArray(board)).Run();
        new P2(position, CopyJaggedArray(board)).Run();
    }

    public static char[][] CopyJaggedArray(char[][] source)
    {
        var len = source.Length;
        var ilen = source[0].Length; //although jagged, all rows have same length. Jagged are quicker and i implement a brute force for p2
        var dest = new char[len][];

        for (var i = 0; i < len; i++)
        {
            var inner = source[i];
            var newer = new char[ilen];
            Array.Copy(inner, newer, ilen);
            dest[i] = newer;
        }

        return dest;
    }
}

public struct Vector
{
    public int X;
    public int Y;

    public Vector(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector operator +(Vector a, Vector b)
    {
        return new Vector(a.X + b.X, a.Y + b.Y);
    }

    public static bool InBounds(Vector a, int maxWidth, int maxHeight)
    {
        if (a.X < 0 || a.Y < 0 || a.X > maxWidth || a.Y > maxHeight)
            return false;
        return true;
    }
}

public struct LocationInfo
{
    int x;
    int y;
    int dir;

    public LocationInfo(int x, int y, int dir)
    {
        this.x = x;
        this.y = y;
        this.dir = dir;
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is LocationInfo))
            return false;

        var item = (LocationInfo) obj;

        if (item.x == this.x && item.y == this.y && item.dir == this.dir)
            return true;
        return false;
    }
}

