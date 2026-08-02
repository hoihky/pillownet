using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>EXIF metadata. Mirrors <c>PIL.Image.Exif</c>.</summary>
public sealed class Exif : IDisposable
{
    private bool _disposed;

    internal Exif(PyObject handle)
    {
        Handle = handle;
    }

    internal PyObject Handle { get; }

    public bool Contains(ExifTag tag) => Contains((long)tag);

    public bool Contains(long tag)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return PillowEnvironment.Bridge.ExifHasTag(Handle, tag);
    }

    public PyObject? Get(ExifTag tag) => Get((long)tag);

    public PyObject? Get(long tag)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var value = PillowEnvironment.Bridge.ExifGetItem(Handle, tag);
        return value is null || value.IsNone() ? null : value;
    }

    public void Set(ExifTag tag, PyObject value) => Set((long)tag, value);

    public void Set(long tag, PyObject value)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        PillowEnvironment.Bridge.ExifSetItem(Handle, tag, value);
    }

    public void Set(ExifTag tag, long value)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        PillowEnvironment.Bridge.ExifSetItem(Handle, (long)tag, PyObject.From(value));
    }

    public void Set(ExifTag tag, string value)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        PillowEnvironment.Bridge.ExifSetItem(Handle, (long)tag, PyObject.From(value));
    }

    public IReadOnlyList<long> Tags
    {
        get
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return PillowEnvironment.Bridge.ExifListTags(Handle);
        }
    }

    public byte[] ToBytes()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return PillowEnvironment.Bridge.ExifToBytes(Handle);
    }

    public IReadOnlyList<(long Tag, PyObject Value)> GetIfdItems(ExifIfd group)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return PillowEnvironment.Bridge.ExifIfdItems(Handle, (long)group)
            .Select(t => (t.Item1, t.Item2))
            .ToArray();
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
}
