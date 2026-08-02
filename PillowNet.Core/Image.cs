using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>
/// Represents a Pillow image. Mirrors <c>PIL.Image.Image</c>.
/// </summary>
public sealed class Image : IDisposable
{
    private bool _disposed;

    internal Image(PyObject handle)
    {
        Handle = handle;
    }

    internal PyObject Handle { get; }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

    /// <summary>Opens an image file. Mirrors <c>PIL.Image.open</c>.</summary>
    public static Image Open(string path) =>
        new(PillowEnvironment.Bridge.ImageOpen(path));

    /// <summary>Creates a new image. Mirrors <c>PIL.Image.new</c>.</summary>
    public static Image New(string mode, (int Width, int Height) size, int color = 0) =>
        new(PillowEnvironment.Bridge.ImageNew(mode, (size.Width, size.Height), color));

    /// <summary>Creates an image from raw bytes. Mirrors <c>PIL.Image.frombytes</c>.</summary>
    public static Image FromBytes(string mode, (int Width, int Height) size, byte[] data) =>
        new(PillowEnvironment.Bridge.ImageFrombytes(mode, (size.Width, size.Height), data));

    public int Width
    {
        get
        {
            ThrowIfDisposed();
            return (int)Handle.GetAttr("width").As<long>();
        }
    }

    public int Height
    {
        get
        {
            ThrowIfDisposed();
            return (int)Handle.GetAttr("height").As<long>();
        }
    }

    public (int Width, int Height) Size
    {
        get
        {
            ThrowIfDisposed();
            var size = Handle.GetAttr("size").As<(long, long)>();
            return ((int)size.Item1, (int)size.Item2);
        }
    }

    public string Mode
    {
        get
        {
            ThrowIfDisposed();
            return Handle.GetAttr("mode").As<string>();
        }
    }

    public string? Format
    {
        get
        {
            ThrowIfDisposed();
            using var format = Handle.GetAttr("format");
            return format.IsNone() ? null : format.As<string>();
        }
    }

    /// <summary>Saves the image. Mirrors <c>Image.save</c>.</summary>
    public void Save(string path, string? format = null, Exif? exif = null)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (format is null && exif is null)
        {
            Handle.GetAttr("save").Call(PyObject.From(path));
            return;
        }

        if (format is not null && exif is null)
        {
            Handle.GetAttr("save").CallWithKeywordArguments(
                args: [PyObject.From(path)],
                kwnames: ["format"],
                kwvalues: [PyObject.From(format)]);
            return;
        }

        if (format is null && exif is not null)
        {
            Handle.GetAttr("save").CallWithKeywordArguments(
                args: [PyObject.From(path)],
                kwnames: ["exif"],
                kwvalues: [PyObject.From(exif.ToBytes())]);
            return;
        }

        Handle.GetAttr("save").CallWithKeywordArguments(
            args: [PyObject.From(path)],
            kwnames: ["format", "exif"],
            kwvalues: [PyObject.From(format!), PyObject.From(exif!.ToBytes())]);
    }

    /// <summary>Resizes the image. Mirrors <c>Image.resize</c>.</summary>
    public Image Resize((int Width, int Height) size, Resampling resample = Resampling.Bicubic)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var result = Handle.GetAttr("resize").Call(
            PyObject.From((size.Width, size.Height)),
            PyObject.From((long)resample));

        return new Image(result);
    }

    /// <summary>
    /// Resizes the image in place preserving aspect ratio.
    /// Mirrors <c>Image.thumbnail</c>.
    /// </summary>
    public void Thumbnail((int Width, int Height) size, Resampling resample = Resampling.Bicubic)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        Handle.GetAttr("thumbnail").Call(
            PyObject.From((size.Width, size.Height)),
            PyObject.From((long)resample));
    }

    /// <summary>Converts the image mode. Mirrors <c>Image.convert</c>.</summary>
    public Image Convert(string mode)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var result = Handle.GetAttr("convert").Call(PyObject.From(mode));
        return new Image(result);
    }

    /// <summary>Applies a filter. Mirrors <c>Image.filter</c>.</summary>
    public Image Filter(IImageFilter filter)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var result = Handle.GetAttr("filter").Call(filter.Handle);
        return new Image(result);
    }

    /// <summary>Rotates the image. Mirrors <c>Image.rotate</c>.</summary>
    public Image Rotate(double angle, Resampling resample = Resampling.Bicubic, bool expand = false)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var result = Handle.GetAttr("rotate").CallWithKeywordArguments(
            args: [PyObject.From(angle)],
            kwnames: ["resample", "expand"],
            kwvalues: [PyObject.From((long)resample), PyObject.From(expand)]);

        return new Image(result);
    }

    /// <summary>Crops the image. Mirrors <c>Image.crop</c>.</summary>
    public Image Crop((int Left, int Top, int Right, int Bottom) box)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var result = Handle.GetAttr("crop").Call(
            PyObject.From((box.Left, box.Top, box.Right, box.Bottom)));

        return new Image(result);
    }

    /// <summary>Returns a copy. Mirrors <c>Image.copy</c>.</summary>
    public Image Copy()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return new Image(Handle.GetAttr("copy").Call());
    }

    /// <summary>Transposes the image. Mirrors <c>Image.transpose</c>.</summary>
    public Image Transpose(Transpose method)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var result = Handle.GetAttr("transpose").Call(PyObject.From((long)method));
        return new Image(result);
    }

    /// <summary>Pastes another image into this one. Mirrors <c>Image.paste</c>.</summary>
    public void Paste(Image im, (int X, int Y)? position = null)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (position is null)
        {
            Handle.GetAttr("paste").Call(im.Handle);
            return;
        }

        Handle.GetAttr("paste").Call(
            im.Handle,
            PyObject.From((position.Value.X, position.Value.Y)));
    }

    /// <summary>Composites with alpha onto this image. Mirrors <c>Image.alpha_composite</c>.</summary>
    public void AlphaComposite(Image im, (int X, int Y) dest = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Handle.GetAttr("alpha_composite").CallWithKeywordArguments(
            args: [im.Handle],
            kwnames: ["dest"],
            kwvalues: [PyObject.From((dest.X, dest.Y))]);
    }

    /// <summary>Splits into individual bands. Mirrors <c>Image.split</c>.</summary>
    public IReadOnlyList<Image> Split()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        using var bands = Handle.GetAttr("split").Call();
        return bands.AsEnumerable<PyObject>().Select(b => new Image(b)).ToArray();
    }

    /// <summary>Merges bands into one image. Mirrors <c>PIL.Image.merge</c>.</summary>
    public static Image Merge(string mode, IReadOnlyList<Image> bands)
    {
        var handles = bands.Select(b => b.Handle).ToArray();
        return new Image(PillowEnvironment.Bridge.ImageMerge(mode, handles));
    }

    /// <summary>Gets EXIF metadata. Mirrors <c>Image.getexif</c>.</summary>
    public Exif GetExif()
    {
        ThrowIfDisposed();
        return new(PillowEnvironment.Bridge.ExifFromImage(Handle));
    }

    /// <summary>Ensures image data is loaded. Mirrors <c>Image.load</c>.</summary>
    public void Load()
    {
        ThrowIfDisposed();
        PillowEnvironment.Bridge.PixelLoad(Handle);
    }

    /// <summary>Gets a pixel value. Mirrors <c>Image.getpixel</c>.</summary>
    public PyObject GetPixel(int x, int y)
    {
        ThrowIfDisposed();
        return PixelAccess.GetPixel(this, x, y);
    }

    /// <summary>Sets a pixel value. Mirrors <c>Image.putpixel</c>.</summary>
    public void PutPixel(int x, int y, long value)
    {
        ThrowIfDisposed();
        PixelAccess.PutPixel(this, x, y, value);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            PillowEnvironment.Bridge.ImageClose(Handle);
        }
        finally
        {
            Handle.Dispose();
            _disposed = true;
        }
    }
}
