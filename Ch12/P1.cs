using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

public class P1
{
    public static int _total = 0;
    public static Dictionary<string, int[]> reigons = new Dictionary<string, int[]>();
    public static List<(int Col, int Row, int Index)> dirs = new List<(int Row, int Col, int Index)>()
    {
        (-1, 0, 0),//up
        (0, -1, 1),//left
        (1, 0, 2),//down
        (0, 1, 3),//right
    };
    public static List<string> content;
    public static List<List<string>> garden;


    public static Dictionary<char, int> idNums = new Dictionary<char, int>();

    public static void Run(List<string> _content)
    {
        content = _content;
        garden = content.Select(x => x.Select(y => y.ToString()).ToList()).ToList();
        var watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < content.Count; i++)
        {
            for (int j = 0; j < content.Count; j++)
            {
                if (garden[i][j].Length > 1)
                    continue;

                var plot = garden[i][j].ToCharArray()[0];
                int id;
                if (!idNums.ContainsKey(plot))
                {
                    id = 0;
                    idNums.Add(plot, id);
                }
                else
                {
                    idNums[plot]++;
                    id = idNums[plot];
                }
                reigons.Add($"{plot}{id}", new int[] { 0, 0 });


                CheckAdj(i, j, -1, id);
            }
        }

        foreach (var value in reigons.Values)
            _total += value[0] * value[1];//0 is perimeter, 1 is area

        watch.Stop();
        Console.WriteLine($"Part 1: {_total}, {watch.ElapsedMilliseconds}ms");
    }

    private static void CheckAdj(int i, int j, int exclude, int id)
    {
        var perimeter = 0;
        var value = garden[i][j];
        if (value.Length > 1)
            return;
        List<int> toCheck = new List<int>() { 0, 1, 2, 3 };//directions to check in
        toCheck.Remove(exclude);
        CheckBounds(content, i, j, ref perimeter, toCheck);
        var connected = new List<(int, int, int)>();
        foreach (var dir in toCheck)
        {
            var direction = dirs[dir];
            int origin = (dir + 2) % 4;
            var val = garden[i + direction.Col][j + direction.Row];
            if (val == value)
                connected.Add((i + direction.Col, j + direction.Row, origin));
            else if (val[0] != value[0])
                perimeter++;
        }
        garden[i][j] = value += id.ToString();
        reigons[value][0] += perimeter;
        reigons[value][1]++;
        foreach (var plot in connected)
            CheckAdj(plot.Item1, plot.Item2, plot.Item3, id);
    }

    private static void CheckBounds(List<string> content, int i, int j, ref int perimeter, List<int> toCheck)
    {
        if (i == 0)
        {
            perimeter++;
            toCheck.Remove(0);
        }
        else if (i == content.Count - 1)
        {
            perimeter++;
            toCheck.Remove(2);
        }

        if (j == 0)
        {
            perimeter++;
            toCheck.Remove(1);
        }
        else if (j == content[0].Length - 1)
        {
            perimeter++;
            toCheck.Remove(3);
        }
    }
}
