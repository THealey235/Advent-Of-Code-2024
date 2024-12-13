using System.Diagnostics;

public class P1
{
    public static int _total;
    public static void Run(List<string> content)
    {
        var watch = new Stopwatch();
        watch.Start();

        watch.Stop();
        Console.WriteLine($"Part 2: {_total}, {watch.ElapsedMilliseconds}ms");
    }
}