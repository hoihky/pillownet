using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>
/// Drawing context. Mirrors <c>PIL.ImageDraw.Draw</c>. Modifies the target image in place.
/// </summary>
public sealed class ImageDraw : IDisposable
{
    private bool _disposed;

    public ImageDraw(Image image)
    {
        Image = image;
        Handle = PillowEnvironment.Bridge.DrawCreate(image.Handle);
    }

    internal PyObject Handle { get; }

    public Image Image { get; }

    public void Rectangle(
        (int Left, int Top, int Right, int Bottom) xy,
        object? fill = null,
        object? outline = null,
        int width = 1)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        PillowEnvironment.Bridge.DrawRectangle(
            Handle,
            (xy.Left, xy.Top, xy.Right, xy.Bottom),
            ToPy(fill),
            ToPy(outline),
            width);
    }

    public void Ellipse(
        (int Left, int Top, int Right, int Bottom) xy,
        object? fill = null,
        object? outline = null,
        int width = 1)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        PillowEnvironment.Bridge.DrawEllipse(
            Handle,
            (xy.Left, xy.Top, xy.Right, xy.Bottom),
            ToPy(fill),
            ToPy(outline),
            width);
    }

    public void Line(
        (int X0, int Y0, int X1, int Y1) xy,
        int fill = 255,
        int width = 1)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        PillowEnvironment.Bridge.DrawLine(
            Handle,
            (xy.X0, xy.Y0, xy.X1, xy.Y1),
            PyObject.From(fill),
            width);
    }

    public void Text(
        (int X, int Y) xy,
        string text,
        int fill = 0,
        string? fontPath = null,
        int fontSize = 20)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        PillowEnvironment.Bridge.DrawText(
            Handle,
            (xy.X, xy.Y),
            text,
            PyObject.From(fill),
            fontPath,
            fontSize);
    }

    public void Text(
        (int X, int Y) xy,
        string text,
        IImageFont font,
        int fill = 0)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        PillowEnvironment.Bridge.DrawTextWithFont(
            Handle,
            (xy.X, xy.Y),
            text,
            font.Handle,
            PyObject.From(fill));
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Handle.Dispose();
        _disposed = true;
    }

    private static PyObject? ToPy(object? value) =>
        value switch
        {
            null => null,
            PyObject py => py,
            _ => PyObject.From(value),
        };
}
