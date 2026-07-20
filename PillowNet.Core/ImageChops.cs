namespace PillowNet;

/// <summary>
/// Channel operations. Mirrors <c>PIL.ImageChops</c>.
/// </summary>
public static class ImageChops
{
    public static Image Difference(Image image1, Image image2) =>
        new(PillowEnvironment.Bridge.ChopsDifference(image1.Handle, image2.Handle));

    public static Image Multiply(Image image1, Image image2) =>
        new(PillowEnvironment.Bridge.ChopsMultiply(image1.Handle, image2.Handle));

    public static Image Add(Image image1, Image image2, double scale = 1.0, long offset = 0) =>
        new(PillowEnvironment.Bridge.ChopsAdd(image1.Handle, image2.Handle, scale, offset));

    public static Image Subtract(Image image1, Image image2, double scale = 1.0, long offset = 0) =>
        new(PillowEnvironment.Bridge.ChopsSubtract(image1.Handle, image2.Handle, scale, offset));

    public static Image Blend(Image image1, Image image2, double alpha) =>
        new(PillowEnvironment.Bridge.ChopsBlend(image1.Handle, image2.Handle, alpha));

    public static Image Composite(Image image1, Image image2, Image mask) =>
        new(PillowEnvironment.Bridge.ChopsComposite(image1.Handle, image2.Handle, mask.Handle));

    public static Image Lighter(Image image1, Image image2) =>
        new(PillowEnvironment.Bridge.ChopsLighter(image1.Handle, image2.Handle));

    public static Image Darker(Image image1, Image image2) =>
        new(PillowEnvironment.Bridge.ChopsDarker(image1.Handle, image2.Handle));

    public static Image Screen(Image image1, Image image2) =>
        new(PillowEnvironment.Bridge.ChopsScreen(image1.Handle, image2.Handle));

    public static Image Overlay(Image image1, Image image2) =>
        new(PillowEnvironment.Bridge.ChopsOverlay(image1.Handle, image2.Handle));
}
