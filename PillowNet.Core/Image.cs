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

    internal bool IsDisposed => _disposed;

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
            using var width = Handle.GetAttr("width");
            return (int)width.As<long>();
        }
    }

    public int Height
    {
        get
        {
            ThrowIfDisposed();
            using var height = Handle.GetAttr("height");
            return (int)height.As<long>();
        }
    }

    public (int Width, int Height) Size
    {
        get
        {
            ThrowIfDisposed();
            using var size = Handle.GetAttr("size");
            var dimensions = size.As<(long, long)>();
            return ((int)dimensions.Item1, (int)dimensions.Item2);
        }
    }

    public string Mode
    {
        get
        {
            ThrowIfDisposed();
            using var mode = Handle.GetAttr("mode");
            return mode.As<string>();
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
        ThrowIfDisposed();

        if (format is null && exif is null)
        {
            using var save = Handle.GetAttr("save");
            save.Call(PyObject.From(path));
            return;
        }

        if (format is not null && exif is null)
        {
            using var save = Handle.GetAttr("save");
            save.CallWithKeywordArguments(
                args: [PyObject.From(path)],
                kwnames: ["format"],
                kwvalues: [PyObject.From(format)]);
            return;
        }

        if (format is null && exif is not null)
        {
            using var save = Handle.GetAttr("save");
            save.CallWithKeywordArguments(
                args: [PyObject.From(path)],
                kwnames: ["exif"],
                kwvalues: [PyObject.From(exif.ToBytes())]);
            return;
        }

        using var saveWithExif = Handle.GetAttr("save");
        saveWithExif.CallWithKeywordArguments(
            args: [PyObject.From(path)],
            kwnames: ["format", "exif"],
            kwvalues: [PyObject.From(format!), PyObject.From(exif!.ToBytes())]);
    }

    /// <summary>Resizes the image. Mirrors <c>Image.resize</c>.</summary>
    public Image Resize((int Width, int Height) size, Resampling resample = Resampling.Bicubic)
    {
        ThrowIfDisposed();

        using var resize = Handle.GetAttr("resize");
        var result = resize.Call(
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
        ThrowIfDisposed();

        using var thumbnail = Handle.GetAttr("thumbnail");
        thumbnail.Call(
            PyObject.From((size.Width, size.Height)),
            PyObject.From((long)resample));
    }

    /// <summary>Converts the image mode. Mirrors <c>Image.convert</c>.</summary>
    public Image Convert(string mode)
    {
        ThrowIfDisposed();
        using var convert = Handle.GetAttr("convert");
        var result = convert.Call(PyObject.From(mode));
        return new Image(result);
    }

    /// <summary>Applies a filter. Mirrors <c>Image.filter</c>.</summary>
    public Image Filter(IImageFilter filter)
    {
        ThrowIfDisposed();
        using var filterMethod = Handle.GetAttr("filter");
        var result = filterMethod.Call(filter.Handle);
        return new Image(result);
    }

    /// <summary>Rotates the image. Mirrors <c>Image.rotate</c>.</summary>
    public Image Rotate(double angle, Resampling resample = Resampling.Bicubic, bool expand = false)
    {
        ThrowIfDisposed();

        using var rotate = Handle.GetAttr("rotate");
        var result = rotate.CallWithKeywordArguments(
            args: [PyObject.From(angle)],
            kwnames: ["resample", "expand"],
            kwvalues: [PyObject.From((long)resample), PyObject.From(expand)]);

        return new Image(result);
    }

    /// <summary>Crops the image. Mirrors <c>Image.crop</c>.</summary>
    public Image Crop((int Left, int Top, int Right, int Bottom) box)
    {
        ThrowIfDisposed();

        using var crop = Handle.GetAttr("crop");
        var result = crop.Call(
            PyObject.From((box.Left, box.Top, box.Right, box.Bottom)));

        return new Image(result);
    }

    /// <summary>Returns a copy. Mirrors <c>Image.copy</c>.</summary>
    public Image Copy()
    {
        ThrowIfDisposed();
        using var copy = Handle.GetAttr("copy");
        return new Image(copy.Call());
    }

    /// <summary>Transposes the image. Mirrors <c>Image.transpose</c>.</summary>
    public Image Transpose(Transpose method)
    {
        ThrowIfDisposed();
        using var transpose = Handle.GetAttr("transpose");
        var result = transpose.Call(PyObject.From((long)method));
        return new Image(result);
    }

    /// <summary>Pastes another image into this one. Mirrors <c>Image.paste</c>.</summary>
    public void Paste(Image im, (int X, int Y)? position = null)
    {
        ThrowIfDisposed();

        using var paste = Handle.GetAttr("paste");
        if (position is null)
        {
            paste.Call(im.Handle);
            return;
        }

        paste.Call(
            im.Handle,
            PyObject.From((position.Value.X, position.Value.Y)));
    }

    /// <summary>Composites with alpha onto this image. Mirrors <c>Image.alpha_composite</c>.</summary>
    public void AlphaComposite(Image im, (int X, int Y) dest = default)
    {
        ThrowIfDisposed();
        using var alphaComposite = Handle.GetAttr("alpha_composite");
        alphaComposite.CallWithKeywordArguments(
            args: [im.Handle],
            kwnames: ["dest"],
            kwvalues: [PyObject.From((dest.X, dest.Y))]);
    }

    /// <summary>Splits into individual bands. Mirrors <c>Image.split</c>.</summary>
    public IReadOnlyList<Image> Split()
    {
        ThrowIfDisposed();
        using var bands = Handle.GetAttr("split").Call();
        return bands.AsEnumerable<PyObject>().Select(b => new Image(b)).ToArray();
    }

    /// <summary>Merges bands into one image. Mirrors <c>PIL.Image.merge</c>.</summary>
    public static Image Merge(string mode, IReadOnlyList<Image> bands)
    {
        ArgumentNullException.ThrowIfNull(bands);
        if (bands.Count == 0)
        {
            throw new ArgumentException("At least one band is required.", nameof(bands));
        }

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

    /// <summary>Sets an RGB pixel value on RGB/RGBA images.</summary>
    public void PutPixel(int x, int y, (int R, int G, int B) value)
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
