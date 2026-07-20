namespace PillowNet;

/// <summary>
/// Ready-made operations on images. Mirrors <c>PIL.ImageOps</c>.
/// </summary>
public static class ImageOps
{
    public static Image Autocontrast(Image image, double cutoff = 0) =>
        new(PillowEnvironment.Bridge.ImageOpsAutocontrast(image.Handle, cutoff));

    public static Image Equalize(Image image) =>
        new(PillowEnvironment.Bridge.ImageOpsEqualize(image.Handle));

    public static Image Grayscale(Image image) =>
        new(PillowEnvironment.Bridge.ImageOpsGrayscale(image.Handle));

    public static Image Invert(Image image) =>
        new(PillowEnvironment.Bridge.ImageOpsInvert(image.Handle));

    public static Image Posterize(Image image, int bits) =>
        new(PillowEnvironment.Bridge.ImageOpsPosterize(image.Handle, bits));

    public static Image Solarize(Image image, int threshold = 128) =>
        new(PillowEnvironment.Bridge.ImageOpsSolarize(image.Handle, threshold));

    public static Image Flip(Image image) =>
        new(PillowEnvironment.Bridge.ImageOpsFlip(image.Handle));

    public static Image Mirror(Image image) =>
        new(PillowEnvironment.Bridge.ImageOpsMirror(image.Handle));

    public static Image ExifTranspose(Image image) =>
        new(PillowEnvironment.Bridge.ImageOpsExifTranspose(image.Handle));

    public static Image Expand(Image image, int border, int fill = 0) =>
        new(PillowEnvironment.Bridge.ImageOpsExpand(image.Handle, border, fill));

    public static Image Colorize(Image image, int black, int white) =>
        new(PillowEnvironment.Bridge.ImageOpsColorize(image.Handle, black, white));

    public static Image Fit(
        Image image,
        (int Width, int Height) size,
        Resampling method = Resampling.Bicubic,
        int color = 0) =>
        new(PillowEnvironment.Bridge.ImageOpsFit(
            image.Handle,
            (size.Width, size.Height),
            (long)method,
            color));

    public static Image Pad(
        Image image,
        (int Width, int Height) size,
        Resampling method = Resampling.Bicubic,
        int color = 0) =>
        new(PillowEnvironment.Bridge.ImageOpsPad(
            image.Handle,
            (size.Width, size.Height),
            (long)method,
            color));

    public static Image Contain(
        Image image,
        (int Width, int Height) size,
        Resampling method = Resampling.Bicubic) =>
        new(PillowEnvironment.Bridge.ImageOpsContain(
            image.Handle,
            (size.Width, size.Height),
            (long)method));

    public static Image Cover(
        Image image,
        (int Width, int Height) size,
        Resampling method = Resampling.Bicubic) =>
        new(PillowEnvironment.Bridge.ImageOpsCover(
            image.Handle,
            (size.Width, size.Height),
            (long)method));
}
