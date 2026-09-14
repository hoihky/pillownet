using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>
/// A Pillow image filter. Mirrors <c>PIL.ImageFilter.Filter</c>.
/// </summary>
public interface IImageFilter : IDisposable
{
    internal PyObject Handle { get; }
}
