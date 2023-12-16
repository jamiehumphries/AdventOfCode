namespace AdventOfCode.Helpers;

public static class MathHelpers
{
    public static int Product<T>(this IEnumerable<T> source, Func<T, int> selector)
    {
        return source.Select(selector).Aggregate(1, (product, n) => product * n);
    }
}