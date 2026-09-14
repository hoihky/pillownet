namespace PillowNet;

/// <summary>Morphological operators. Mirrors <c>PIL.ImageMorph</c>.</summary>
public static class ImageMorph
{
    /// <summary>Built-in morphology operator names supported by Pillow.</summary>
    public static class Operators
    {
        public const string Erosion4 = "erosion4";
        public const string Erosion8 = "erosion8";
        public const string Dilation4 = "dilation4";
        public const string Dilation8 = "dilation8";
        public const string Edge = "edge";
        public const string Corner = "corner";
    }

    /// <summary>Applies a morphology operator. Returns changed pixel count and result image.</summary>
    public static (int ChangedPixels, Image Result) Apply(Image image, string operatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(operatorName);
        var result = PillowEnvironment.Bridge.MorphopApply(image.Handle, operatorName);
        return ((int)result.Item1, new Image(result.Item2));
    }

    /// <summary>Returns coordinates that match the morphology operator.</summary>
    public static IReadOnlyList<(int X, int Y)> Match(Image image, string operatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(operatorName);
        return PillowEnvironment.Bridge.MorphopMatch(image.Handle, operatorName)
            .Select(p => ((int)p.Item1, (int)p.Item2))
            .ToArray();
    }
}
