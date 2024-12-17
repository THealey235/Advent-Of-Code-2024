using System.Diagnostics;

string[][] content;
using (var reader = new System.IO.StreamReader("input.txt"))
{
    content = reader.ReadToEnd().Split("\r\n\r\n").Select(x =>x.Split("\r\n")).ToArray();
}

var found = false;
var lock _lock = new object();  
var watch = new Stopwatch();
watch.Start();
Console.WriteLine($"Part 1: {Part1(-1)} --- {watch.ElapsedMilliseconds}ms");
watch.Restart();
Console.WriteLine($"Part 2: {Part2()}, {watch.ElapsedMilliseconds}ms");



string Part1(Int64 a)
{
    Int64 A; Int64 B; Int64 C; List<Int64> Program;
    ParseInput(content, out A, out B, out C, out Program);
    List<Int64> outs = Run(Program, A, B, C);

    watch.Stop();
    return string.Join(',', outs);
}
//Brute force...
Int64 Part2()
{
    Int64 A; Int64 B; Int64 C; List<Int64> Program;
    ParseInput(content, out A, out B, out C, out Program);
    var output = new List<Int64>();
    A = 0;
    while (!Program.SequenceEqual(output))
    {
        output = RunP2(Program, A, B, C);
        A++;
        if (A % 20000000 == 0)
            Console.WriteLine(A);
    }

    watch.Stop();
    return A;
}

List<Int64> Run(List<Int64> Program, Int64 A, Int64 B, Int64 C)
{
    List<Int64> outs = new List<Int64>();
    for (Int64 i = 0; i < Program.Count - 1; i++)
    {
        var operation = Program[(int)i];
        i++;
        var operand = Program[(int)i];
        ExecuteOperation(operation, operand, ref A, ref B, ref C, ref i, outs);
    }
    return outs;
}
List<Int64> RunP2(List<Int64> Program, Int64 A, Int64 B, Int64 C)
{
    List<Int64> outs = new List<Int64>();
    for (Int64 i = 0; i < Program.Count - 1; i++)
    {
        var operation = Program[(int)i];
        i++;
        var operand = Program[(int)i];
        ExecuteOperation(operation, operand, ref A, ref B, ref C, ref i, outs);

    }
    return outs;
}

void ExecuteOperation(Int64 operation, Int64 operand, ref Int64 a, ref Int64 b, ref Int64 c, ref Int64 i, List<Int64> outs)
{
    var combo = GetCombo(operand, a, b, c);
    switch (operation)
    {
        case 0:
            a = (Int64)(a / Math.Pow((double)2, (double)combo)); break;
        case 1:
            b ^= operand; break;
        case 2:
            b = combo % 8; break;
        case 3:
            if (a != 0) i = operand - 1;
            break;
        case 4:
            b ^= c; break;
        case 5:
            outs.Add(combo % 8); break;
        case 6:
            b = (Int64)(a / Math.Pow((double)2, (double)combo)); break;
        case 7:
            c = (Int64)(a / Math.Pow((double)2, (double)combo)); break;
    }
}

Int64 GetCombo(Int64 operand, Int64 a, Int64 b, Int64 c)
{
    if (operand <= 3)
        return operand;
    return (operand) switch
    {
        4 => a,
        5 => b,
        6 => c,
        7 => -1
    };
}

void ParseInput(string[][] x, out Int64 a, out Int64 b, out Int64 c, out List<Int64> program)
{
    a = Int64.Parse(x[0][0].Split(": ")[1]);
    b = Int64.Parse(x[0][1].Split(": ")[1]);
    c = Int64.Parse(x[0][2].Split(": ")[1]);

    program = x[1][0].Split(": ")[1].Split(',').Select(Int64.Parse).ToList();
}