using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>A Pillow font. Mirrors <c>PIL.ImageFont.FreeTypeFont</c>.</summary>
public interface IImageFont
{
    internal PyObject Handle { get; }
}
