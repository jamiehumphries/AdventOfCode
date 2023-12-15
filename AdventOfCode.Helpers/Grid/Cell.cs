namespace AdventOfCode.Helpers.Grid;

public record struct Cell(char[][] Grid, int I, int J)
{
    public char Value { get; } = Grid[I][J];

    public readonly IEnumerable<Cell> Neighbours()
    {
        for (var i = I - 1; i <= I + 1; i++)
        {
            if (i < 0 || i >= Grid.Length)
            {
                continue;
            }

            for (var j = J - 1; j <= J + 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (j < 0 || j >= Grid.Length)
                {
                    continue;
                }

                yield return new Cell(Grid, i, j);
            }
        }
    }
}