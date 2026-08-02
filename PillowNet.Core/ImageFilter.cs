using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>
/// Predefined image filters. Mirrors <c>PIL.ImageFilter</c>.
/// </summary>
public static class ImageFilter
{
    private static readonly Lazy<IImageFilter> BlurFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterBlur()));

    private static readonly Lazy<IImageFilter> ContourFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterContour()));

    private static readonly Lazy<IImageFilter> DetailFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterDetail()));

    private static readonly Lazy<IImageFilter> EdgeEnhanceFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterEdgeEnhance()));

    private static readonly Lazy<IImageFilter> EdgeEnhanceMoreFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterEdgeEnhanceMore()));

    private static readonly Lazy<IImageFilter> EmbossFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterEmboss()));

    private static readonly Lazy<IImageFilter> FindEdgesFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterFindEdges()));

    private static readonly Lazy<IImageFilter> SharpenFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterSharpen()));

    private static readonly Lazy<IImageFilter> SmoothFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterSmooth()));

    private static readonly Lazy<IImageFilter> SmoothMoreFilter =
        new(() => FromBridge(PillowEnvironment.Bridge.FilterSmoothMore()));

    public static IImageFilter Blur => BlurFilter.Value;

    public static IImageFilter Contour => ContourFilter.Value;

    public static IImageFilter Detail => DetailFilter.Value;

    public static IImageFilter EdgeEnhance => EdgeEnhanceFilter.Value;

    public static IImageFilter EdgeEnhanceMore => EdgeEnhanceMoreFilter.Value;

    public static IImageFilter Emboss => EmbossFilter.Value;

    public static IImageFilter FindEdges => FindEdgesFilter.Value;

    public static IImageFilter Sharpen => SharpenFilter.Value;

    public static IImageFilter Smooth => SmoothFilter.Value;

    public static IImageFilter SmoothMore => SmoothMoreFilter.Value;

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
