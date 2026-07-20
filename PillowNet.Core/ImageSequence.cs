using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>Multi-frame image access. Mirrors <c>PIL.ImageSequence</c>.</summary>
public static class ImageSequence
{
    /// <summary>Returns a copy of every frame. Mirrors <c>ImageSequence.all_frames</c>.</summary>
    public static IReadOnlyList<Image> AllFrames(Image image) =>
        PillowEnvironment.Bridge.SequenceAllFrames(image.Handle)
            .Select(h => new Image(h))
            .ToArray();

    /// <summary>Counts frames in an animated image.</summary>
    public static int FrameCount(Image image) =>
        (int)PillowEnvironment.Bridge.SequenceFrameCount(image.Handle);
}
