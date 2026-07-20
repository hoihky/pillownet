using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>Font loading and metrics. Mirrors <c>PIL.ImageFont</c>.</summary>
public static class ImageFont
{
    public static IImageFont Truetype(string path, double size = 10) =>
        new ImageFontHandle(PillowEnvironment.Bridge.FontTruetype(path, size));

    public static IImageFont LoadDefault(double? size = null) =>
        new ImageFontHandle(PillowEnvironment.Bridge.FontLoadDefault(size));

    public static (int Left, int Top, int Right, int Bottom) GetBBox(IImageFont font, string text)
    {
        var box = PillowEnvironment.Bridge.FontGetbbox(font.Handle, text);
        return ((int)box.Item1, (int)box.Item2, (int)box.Item3, (int)box.Item4);
    }

    public static double GetLength(IImageFont font, string text) =>
        PillowEnvironment.Bridge.FontGetlength(font.Handle, text);

    private sealed class ImageFontHandle(PyObject handle) : IImageFont
    {
        public PyObject Handle { get; } = handle;
    }
}
