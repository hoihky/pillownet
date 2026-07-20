using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>
/// Image statistics. Mirrors <c>PIL.ImageStat.Stat</c>.
/// </summary>
public static class ImageStat
{
    public static IReadOnlyList<double> Mean(Image image) =>
        PillowEnvironment.Bridge.StatMean(image.Handle)
            .As<IReadOnlyList<double>>();

    public static IReadOnlyList<(int Min, int Max)> Extrema(Image image) =>
        PillowEnvironment.Bridge.StatExtrema(image.Handle)
            .As<IReadOnlyList<(long, long)>>()
            .Select(t => ((int)t.Item1, (int)t.Item2))
            .ToArray();

    public static IReadOnlyList<long> Count(Image image) =>
        PillowEnvironment.Bridge.StatCount(image.Handle)
            .As<IReadOnlyList<long>>();
}
