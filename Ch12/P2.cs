using System.Diagnostics;
using System.Reflection;

//holy this code is actually unreadable i apologise
public class P2
{
    public static int _total = 0;
    public static Dictionary<string, List<Plot>> reigons = new Dictionary<string, List<Plot>>();
    public static List<(int Col, int Row, int Index)> dirs = new List<(int Row, int Col, int Index)>()
    {
        (-1, 0, 0),//up
        (0, -1, 1),//left
        (1, 0, 2),//down
        (0, 1, 3),//right
    };
    public static List<string> content;
    public static List<List<string>> garden;
    public static List<List<bool[]>> isCheckedMap = new List<List<bool[]>>();
    public static List<List<bool[]>> sidesMap = new List<List<bool[]>>();
    public static Dictionary<char, int> idNums = new Dictionary<char, int>();

    public static void Run(List<string> _content)
    {
        var watch = new Stopwatch();
        watch.Start();

        content = _content;
        garden = content.Select(x => x.Select(y => y.ToString()).ToList()).ToList();
        for (int i = 0; i < content.Count; i++)
        {
            isCheckedMap.Add(new List<bool[]>());
            for (int j = 0; j < content[0].Length; j++)
            isCheckedMap[i].Add(new bool[] { false, false, false, false, false});
        }

                
        for (int i = 0; i < garden.Count; i++) 
        {
            sidesMap.Add(new List<bool[]>());
            for (int j = 0; j < garden[i].Count; j++)
                sidesMap[i].Add(new bool[] { false, false, false, false });
        }

        

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
                reigons.Add($"{plot}{id}", new List<Plot>());
                CheckAdj(i, j, id);
            }
        }

        var reigonSides = new Dictionary<string, int>();
        foreach (var (value, plots) in reigons)
        {
            reigonSides.Add(value, 0);
            var curProcess = new List<Plot>() { plots[0] };
            var nextProcess = new List<Plot>();
            while (curProcess.Count != 0)
            {
                foreach (var plot in curProcess)
                {
                    reigonSides[value] += Search(sidesMap[plot.Row][plot.Col], plot.Connected, value);
                    isCheckedMap[plot.Row][plot.Col] = new bool[] { true, true, true, true, true };
                    foreach (var p in plot.Connected)
                    {
                        if (isCheckedMap[p.Row][p.Col][^1])
                            continue;
                        var cons = GenerateConnected(p.Row, p.Col);
                        nextProcess.Add(new Plot(p.Row, p.Col, cons));
                    }
                }
                curProcess = nextProcess.Distinct().ToList();
                nextProcess.Clear();
            }
        }

        foreach (var (value, plots) in reigons)
        {
            _total += reigonSides[value] * plots.Count;
        }
        watch.Stop();
        Console.WriteLine($"DOESN'T WORK ---- Part 2: {_total}, {watch.ElapsedMilliseconds}ms");
    }

    private static List<(int Row, int Col)> GenerateConnected(int row, int col)
    {
        var cons = new List<(int Row, int Col)>();
        var sides = sidesMap[row][col];
        for (int i = 0; i <  sides.Length; i++)
            if (!sides[i])
            {
                var dir = dirs[i];
                var r = row + dir.Row;
                var c = col + dir.Col;
                if (r < 0 || r >= content.Count || c < 0 || c >= content[0].Length)
                    continue;
                cons.Add((r, c));
            }
        return cons;
    }

    private static void CheckAdj(int i, int j, int id)
    {
        var value = garden[i][j];
        var connected = new List<(int Row, int Col)>();
        var boundaries = new Dictionary<int, bool>() { { 0, false }, { 1, false }, { 2, false }, { 3, false } };
        List<int> toCheck = new List<int>() { 0, 1, 2, 3 };//directions to check in

        if (value.Length > 1)
            return;
        CheckBounds(content, i, j, boundaries, toCheck);
        FindConnectedAndBoundaries(i, j, value, toCheck, connected, boundaries);

        garden[i][j] = value += id.ToString();
        reigons[value].Add(new Plot(i, j, connected));
        var sides = boundaries.Values.ToList();
        sidesMap[i][j] = sides.ToArray();

        foreach (var plot in connected)
        {
            if (sidesMap[plot.Row][plot.Col][0])
                continue;
            CheckAdj(plot.Item1, plot.Item2, id);
        }
    }

    public struct Plot
    {
        public int Row;
        public int Col;
        public List<(int Col, int Row)> Connected;

        public Plot(int row, int col, List<(int Col, int Row)> connected)
        {
            Row = row;
            Col = col;
            Connected = connected;
        }
    }

    private static void FindConnectedAndBoundaries(int i, int j, string value, List<int> toCheck, List<(int, int)> connected, Dictionary<int, bool> boundaries)
    {
        foreach (var dir in toCheck)
        {
            var direction = dirs[dir];
            var val = garden[i + direction.Col][j + direction.Row];
            if (val[0] == value[0])
                connected.Add((i + direction.Col, j + direction.Row));
            else
                boundaries[dir] = true;
        }
    }

    private static void CheckBounds(List<string> content, int i, int j, Dictionary<int, bool> boundaries, List<int> toCheck)
    {
        if (i == 0)
        {
            boundaries[0] = true;
            toCheck.Remove(0);
        }
        else if (i == content.Count - 1)
        {
            boundaries[2] = true;
            toCheck.Remove(2);
        }

        if (j == 0)
        {
            boundaries[1] = true;
            toCheck.Remove(1);
        }
        else if (j == content[0].Length - 1)
        {
            boundaries[3] = true;
            toCheck.Remove(3);
        }
    }

    private static int Search(bool[] boundaries, List<(int Row, int Col)> connected, string value)
    {
        var total = 0;
        for (int i = 0; i < boundaries.Length; i++)
        {
            var state = boundaries[i];
            if (!state)
                continue;
            var flag = false;
            foreach (var connPlot in connected)
            {
                var connVal = sidesMap[connPlot.Row][connPlot.Col];
                if (!isCheckedMap[connPlot.Row][connPlot.Col][i])
                    flag = true;
                else if (connVal[i] && isCheckedMap[connPlot.Row][connPlot.Col][i])
                {
                    flag = false;
                    break;
                }
                else if (!connVal[i])
                    flag = true;
            }
            if (flag || connected.Count == 0)
                total++;
            foreach (var connPlot in connected)
                isCheckedMap[connPlot.Row][connPlot.Col][i] = true;
        }


        return total;
    }


}