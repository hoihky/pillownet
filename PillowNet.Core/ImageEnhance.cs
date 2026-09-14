using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>
/// Image enhancement. Mirrors <c>PIL.ImageEnhance</c>.
/// </summary>
public static class ImageEnhance
{
    /// <summary>Adjust brightness. Factor 1.0 = original, 0.0 = black.</summary>
    public static Image Brightness(Image image, double factor) =>
        new(PillowEnvironment.Bridge.EnhanceBrightness(image.Handle, factor));

    /// <summary>Adjust contrast. Factor 1.0 = original, 0.0 = flat gray.</summary>
    public static Image Contrast(Image image, double factor) =>
        new(PillowEnvironment.Bridge.EnhanceContrast(image.Handle, factor));

    /// <summary>Adjust color saturation. Factor 1.0 = original, 0.0 = grayscale.</summary>
    public static Image Color(Image image, double factor) =>
        new(PillowEnvironment.Bridge.EnhanceColor(image.Handle, factor));

    /// <summary>Adjust sharpness. Factor 1.0 = original, 2.0 = sharpened.</summary>
    public static Image Sharpness(Image image, double factor) =>
        new(PillowEnvironment.Bridge.EnhanceSharpness(image.Handle, factor));
}

/// <summary>Adjust brightness. Mirrors <c>ImageEnhance.Brightness</c>.</summary>
public sealed class BrightnessEnhancer : IDisposable
{
    private readonly PyEnhancer _enhancer;

    public BrightnessEnhancer(Image image) =>
        _enhancer = new PyEnhancer(PillowEnvironment.Bridge.EnhanceBrightnessCreate(image.Handle));

    public Image Enhance(double factor) => _enhancer.Apply(factor);

    public void Dispose() => _enhancer.Dispose();
}

/// <summary>Adjust contrast. Mirrors <c>ImageEnhance.Contrast</c>.</summary>
public sealed class ContrastEnhancer : IDisposable
{
    private readonly PyEnhancer _enhancer;

    public ContrastEnhancer(Image image) =>
        _enhancer = new PyEnhancer(PillowEnvironment.Bridge.EnhanceContrastCreate(image.Handle));

    public Image Enhance(double factor) => _enhancer.Apply(factor);

    public void Dispose() => _enhancer.Dispose();
}

/// <summary>Adjust color balance. Mirrors <c>ImageEnhance.Color</c>.</summary>
public sealed class ColorEnhancer : IDisposable
{
    private readonly PyEnhancer _enhancer;

    public ColorEnhancer(Image image) =>
        _enhancer = new PyEnhancer(PillowEnvironment.Bridge.EnhanceColorCreate(image.Handle));

    public Image Enhance(double factor) => _enhancer.Apply(factor);

    public void Dispose() => _enhancer.Dispose();
}

/// <summary>Adjust sharpness. Mirrors <c>ImageEnhance.Sharpness</c>.</summary>
public sealed class SharpnessEnhancer : IDisposable
{
    private readonly PyEnhancer _enhancer;

    public SharpnessEnhancer(Image image) =>
        _enhancer = new PyEnhancer(PillowEnvironment.Bridge.EnhanceSharpnessCreate(image.Handle));

    public Image Enhance(double factor) => _enhancer.Apply(factor);

    public void Dispose() => _enhancer.Dispose();
}

internal sealed class PyEnhancer : IDisposable
{
    private bool _disposed;

    internal PyEnhancer(PyObject handle)
    {
        Handle = handle;
    }

    internal PyObject Handle { get; }

    internal Image Apply(double factor) =>
        new(PillowEnvironment.Bridge.EnhanceApply(Handle, factor));

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
