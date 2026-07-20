using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>EXIF metadata. Mirrors <c>PIL.Image.Exif</c>.</summary>
public sealed class Exif
{
    internal Exif(PyObject handle)
    {
        Handle = handle;
    }

    internal PyObject Handle { get; }

    public bool Contains(ExifTag tag) => Contains((long)tag);

    public bool Contains(long tag) =>
        PillowEnvironment.Bridge.ExifHasTag(Handle, tag);

    public PyObject? Get(ExifTag tag) => Get((long)tag);

    public PyObject? Get(long tag)
    {
        var value = PillowEnvironment.Bridge.ExifGetItem(Handle, tag);
        return value.IsNone() ? null : value;
    }

    public void Set(ExifTag tag, PyObject value) =>
        PillowEnvironment.Bridge.ExifSetItem(Handle, (long)tag, value);

    public void Set(ExifTag tag, long value) =>
        PillowEnvironment.Bridge.ExifSetItem(Handle, (long)tag, PyObject.From(value));

    public void Set(ExifTag tag, string value) =>
        PillowEnvironment.Bridge.ExifSetItem(Handle, (long)tag, PyObject.From(value));

    public IReadOnlyList<long> Tags =>
        PillowEnvironment.Bridge.ExifListTags(Handle);

    public byte[] ToBytes() => PillowEnvironment.Bridge.ExifToBytes(Handle);

    public IReadOnlyList<(long Tag, PyObject Value)> GetIfdItems(ExifIfd group) =>
        PillowEnvironment.Bridge.ExifIfdItems(Handle, (long)group)
            .Select(t => (t.Item1, t.Item2))
            .ToArray();
}
