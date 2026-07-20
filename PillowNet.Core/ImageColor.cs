namespace PillowNet;

/// <summary>Color parsing. Mirrors <c>PIL.ImageColor</c>.</summary>
public static class ImageColor
{
    /// <summary>Parses a color string to RGB(A) components. Mirrors <c>ImageColor.getrgb</c>.</summary>
    public static IReadOnlyList<int> GetRgb(string color) =>
        PillowEnvironment.Bridge.ColorGetrgb(color)
            .As<IReadOnlyList<long>>()
            .Select(v => (int)v)
            .ToArray();

    /// <summary>Parses a color for a given image mode. Mirrors <c>ImageColor.getcolor</c>.</summary>
    public static long GetColor(string color, string mode) =>
        PillowEnvironment.Bridge.ColorGetcolor(color, mode).As<long>();
}
