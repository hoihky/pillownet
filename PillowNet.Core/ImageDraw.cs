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
        ArgumentNullException.ThrowIfNull(image);
        ObjectDisposedException.ThrowIf(image.IsDisposed, image);
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
        WithPy(fill, fillPy =>
            WithPy(outline, outlinePy =>
                PillowEnvironment.Bridge.DrawRectangle(
                    Handle,
                    (xy.Left, xy.Top, xy.Right, xy.Bottom),
                    fillPy,
                    outlinePy,
                    width)));
    }

    public void Ellipse(
        (int Left, int Top, int Right, int Bottom) xy,
        object? fill = null,
        object? outline = null,
        int width = 1)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        WithPy(fill, fillPy =>
            WithPy(outline, outlinePy =>
                PillowEnvironment.Bridge.DrawEllipse(
                    Handle,
                    (xy.Left, xy.Top, xy.Right, xy.Bottom),
                    fillPy,
                    outlinePy,
                    width)));
    }

    public void Line(
        (int X0, int Y0, int X1, int Y1) xy,
        object? fill = null,
        int width = 1)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        WithPy(fill ?? 0, fillPy =>
            PillowEnvironment.Bridge.DrawLine(
                Handle,
                (xy.X0, xy.Y0, xy.X1, xy.Y1),
                fillPy!,
                width));
    }

    public void Text(
        (int X, int Y) xy,
        string text,
        object? fill = null,
        string? fontPath = null,
        int fontSize = 20)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        WithPy(fill ?? 0, fillPy =>
            PillowEnvironment.Bridge.DrawText(
                Handle,
                (xy.X, xy.Y),
                text,
                fillPy!,
                fontPath,
                fontSize));
    }

    public void Text(
        (int X, int Y) xy,
        string text,
        IImageFont font,
        object? fill = null)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        WithPy(fill ?? 0, fillPy =>
            PillowEnvironment.Bridge.DrawTextWithFont(
                Handle,
                (xy.X, xy.Y),
                text,
                font.Handle,
                fillPy!));
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

    private static void WithPy(object? value, Action<PyObject?> action)
    {
        if (value is null)
        {
            action(null);
            return;
        }

        if (value is PyObject existing)
        {
            action(existing);
            return;
        }

        using var created = PyObject.From(value);
        action(created);
    }
}
