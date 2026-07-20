using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>Direct pixel read/write. Mirrors <c>Image.getpixel</c> / <c>putpixel</c>.</summary>
public static class PixelAccess
{
    public static void Load(Image image) =>
        PillowEnvironment.Bridge.PixelLoad(image.Handle);

    public static PyObject GetPixel(Image image, int x, int y) =>
        PillowEnvironment.Bridge.PixelGet(image.Handle, x, y);

    public static int GetPixelGray(Image image, int x, int y) =>
        (int)PillowEnvironment.Bridge.PixelGet(image.Handle, x, y).As<long>();

    public static (int R, int G, int B) GetPixelRgb(Image image, int x, int y)
    {
        var pixel = PillowEnvironment.Bridge.PixelGet(image.Handle, x, y).As<(long, long, long)>();
        return ((int)pixel.Item1, (int)pixel.Item2, (int)pixel.Item3);
    }

    public static void PutPixel(Image image, int x, int y, long value) =>
        PillowEnvironment.Bridge.PixelSet(image.Handle, x, y, PyObject.From(value));

    public static void PutPixel(Image image, int x, int y, (int R, int G, int B) value) =>
        PillowEnvironment.Bridge.PixelSet(
            image.Handle,
            x,
            y,
            PyObject.From((value.R, value.G, value.B)));
}
