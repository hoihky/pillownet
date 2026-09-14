using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>
/// Predefined image filters. Mirrors <c>PIL.ImageFilter</c>.
/// </summary>
public static class ImageFilter
{
    private static readonly Lazy<IImageFilter> BlurFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterBlur()));

    private static readonly Lazy<IImageFilter> ContourFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterContour()));

    private static readonly Lazy<IImageFilter> DetailFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterDetail()));

    private static readonly Lazy<IImageFilter> EdgeEnhanceFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterEdgeEnhance()));

    private static readonly Lazy<IImageFilter> EdgeEnhanceMoreFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterEdgeEnhanceMore()));

    private static readonly Lazy<IImageFilter> EmbossFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterEmboss()));

    private static readonly Lazy<IImageFilter> FindEdgesFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterFindEdges()));

    private static readonly Lazy<IImageFilter> SharpenFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterSharpen()));

    private static readonly Lazy<IImageFilter> SmoothFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterSmooth()));

    private static readonly Lazy<IImageFilter> SmoothMoreFilter =
        new(() => Cached(PillowEnvironment.Bridge.FilterSmoothMore()));

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
        Transient(PillowEnvironment.Bridge.FilterGaussianBlur(radius));

    public static IImageFilter BoxBlur(double radius) =>
        Transient(PillowEnvironment.Bridge.FilterBoxBlur(radius));

    public static IImageFilter UnsharpMask(double radius = 2.0, long percent = 150, long threshold = 3) =>
        Transient(PillowEnvironment.Bridge.FilterUnsharpMask(radius, percent, threshold));

    public static IImageFilter Min(long size = 3) =>
        Transient(PillowEnvironment.Bridge.FilterMin(size));

    public static IImageFilter Max(long size = 3) =>
        Transient(PillowEnvironment.Bridge.FilterMax(size));

    public static IImageFilter Median(long size = 3) =>
        Transient(PillowEnvironment.Bridge.FilterMedian(size));

    public static IImageFilter Mode(long size = 3) =>
        Transient(PillowEnvironment.Bridge.FilterModeFilter(size));

    private static IImageFilter Cached(PyObject handle) => new CachedImageFilterHandle(handle);

    private static IImageFilter Transient(PyObject handle) => new TransientImageFilterHandle(handle);

    private sealed class CachedImageFilterHandle(PyObject handle) : IImageFilter
    {
        public PyObject Handle { get; } = handle;

        public void Dispose()
        {
        }
    }

    private sealed class TransientImageFilterHandle(PyObject handle) : IImageFilter
    {
        private bool _disposed;

        public PyObject Handle { get; } = handle;

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
}
