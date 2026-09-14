using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>Direct pixel read/write. Mirrors <c>Image.getpixel</c> / <c>putpixel</c>.</summary>
public static class PixelAccess
{
    public static void Load(Image image) =>
        PillowEnvironment.Bridge.PixelLoad(image.Handle);

    public static PyObject GetPixel(Image image, int x, int y) =>
        PillowEnvironment.Bridge.PixelGet(image.Handle, x, y);

    public static int GetPixelGray(Image image, int x, int y)
    {
        using var pixel = PillowEnvironment.Bridge.PixelGet(image.Handle, x, y);
        return (int)pixel.As<long>();
    }

    public static (int R, int G, int B) GetPixelRgb(Image image, int x, int y)
    {
        using var pixel = PillowEnvironment.Bridge.PixelGet(image.Handle, x, y);
        var components = pixel.As<(long, long, long)>();
        return ((int)components.Item1, (int)components.Item2, (int)components.Item3);
    }

    public static void PutPixel(Image image, int x, int y, long value)
    {
        using var pixel = PyObject.From(value);
        PillowEnvironment.Bridge.PixelSet(image.Handle, x, y, pixel);
    }

    public static void PutPixel(Image image, int x, int y, (int R, int G, int B) value)
    {
        using var pixel = PyObject.From((value.R, value.G, value.B));
        PillowEnvironment.Bridge.PixelSet(image.Handle, x, y, pixel);
    }
}
