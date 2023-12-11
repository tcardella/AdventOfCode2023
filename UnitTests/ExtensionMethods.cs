namespace UnitTests;

public static class ExtensionMethods
{
    public static bool OverlapsWith(this Range r1, MapRange r2) => r1.End >= r2.SourceStart && r1.Start <= r2.SourceEnd;

    //public static Range GetOverlap(this Range r1, MapRange r2)
    //{
    //    var overlapRange = new Range(Math.Max(r2.SourceStart, r1.Start),
    //        Math.Min(r2.SourceEnd, r1.End));

    //    var start = overlapRange.Start - r2.SourceStart;
    //    var end = overlapRange.End - r2.SourceEnd;

    //    return new Range(r2.DestinationStart + start, r2.DestinationEnd + end);
    //}

    public static Range GetOverlap(this Range r1, MapRange r2)
    {
        var overlapRange = new Range(Math.Max(r2.SourceStart, r1.Start),
            Math.Min(r2.SourceEnd, r1.End));

        return overlapRange;
    }

    public static string Dump(this Range input) => input.ToString();

    public static string Dump(this IEnumerable<Range> input) => string.Join(',', input);
}