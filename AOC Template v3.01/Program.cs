using System.Diagnostics;

List<string> content;
using (var reader = new System.IO.StreamReader("input.txt"))
{
    content = reader.ReadToEnd().Split("\r\n").ToList();
}

var watch = new Stopwatch();
watch.Start();
Console.WriteLine($"Part 1: {Part1(content)}, {watch.ElapsedMilliseconds}ms");
watch.Restart();
Console.WriteLine($"Part 2: {Part2(content)}, {watch.ElapsedMilliseconds}ms");



int Part1(List<string> content)
{
    var total = 0;

    watch.Stop();
    return total;
}

int Part2(List<string> content)
{
    var total = 0;

    watch.Stop();
    return total;
}