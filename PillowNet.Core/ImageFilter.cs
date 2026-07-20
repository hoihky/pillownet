using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>
/// Predefined image filters. Mirrors <c>PIL.ImageFilter</c>.
/// </summary>
public static class ImageFilter
{
    public static IImageFilter Blur => FromBridge(PillowEnvironment.Bridge.FilterBlur());

    public static IImageFilter Contour => FromBridge(PillowEnvironment.Bridge.FilterContour());

    public static IImageFilter Detail => FromBridge(PillowEnvironment.Bridge.FilterDetail());

    public static IImageFilter EdgeEnhance => FromBridge(PillowEnvironment.Bridge.FilterEdgeEnhance());

    public static IImageFilter EdgeEnhanceMore => FromBridge(PillowEnvironment.Bridge.FilterEdgeEnhanceMore());

    public static IImageFilter Emboss => FromBridge(PillowEnvironment.Bridge.FilterEmboss());

    public static IImageFilter FindEdges => FromBridge(PillowEnvironment.Bridge.FilterFindEdges());

    public static IImageFilter Sharpen => FromBridge(PillowEnvironment.Bridge.FilterSharpen());

    public static IImageFilter Smooth => FromBridge(PillowEnvironment.Bridge.FilterSmooth());

    public static IImageFilter SmoothMore => FromBridge(PillowEnvironment.Bridge.FilterSmoothMore());

    public static IImageFilter GaussianBlur(double radius = 2.0) =>
        FromBridge(PillowEnvironment.Bridge.FilterGaussianBlur(radius));

    public static IImageFilter BoxBlur(double radius) =>
        FromBridge(PillowEnvironment.Bridge.FilterBoxBlur(radius));

    public static IImageFilter UnsharpMask(double radius = 2.0, long percent = 150, long threshold = 3) =>
        FromBridge(PillowEnvironment.Bridge.FilterUnsharpMask(radius, percent, threshold));

    public static IImageFilter Min(long size = 3) =>
        FromBridge(PillowEnvironment.Bridge.FilterMin(size));

    public static IImageFilter Max(long size = 3) =>
        FromBridge(PillowEnvironment.Bridge.FilterMax(size));

    public static IImageFilter Median(long size = 3) =>
        FromBridge(PillowEnvironment.Bridge.FilterMedian(size));

    public static IImageFilter Mode(long size = 3) =>
        FromBridge(PillowEnvironment.Bridge.FilterModeFilter(size));

    private static ImageFilterHandle FromBridge(PyObject handle) => new(handle);

    private sealed class ImageFilterHandle(PyObject handle) : IImageFilter
    {
        public PyObject Handle { get; } = handle;
    }
}
