namespace AdventOfCode.Helpers.Parsing;

public class LineWithId
{
    public LineWithId(string prefix, string line)
    {
        var split = line.Split(": ");
        Id = int.Parse(split[0][(prefix + " ").Length..]);
        Data = split[1];
    }

    public int Id { get; }
    public string Data { get; }
}