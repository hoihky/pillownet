namespace PillowNet;

/// <summary>Geometric transforms. Mirrors <c>PIL.ImageTransform</c>.</summary>
public static class ImageTransform
{
    public static Image Affine(
        Image image,
        (int Width, int Height) size,
        (double A, double B, double C, double D, double E, double F) matrix,
        Resampling resample = Resampling.Bicubic) =>
        new(PillowEnvironment.Bridge.TransformAffine(
            image.Handle,
            (size.Width, size.Height),
            (matrix.A, matrix.B, matrix.C, matrix.D, matrix.E, matrix.F),
            (long)resample));

    public static Image Perspective(
        Image image,
        (int Width, int Height) size,
        (
            double A, double B, double C, double D,
            double E, double F, double G, double H
        ) matrix,
        Resampling resample = Resampling.Bicubic) =>
        new(PillowEnvironment.Bridge.TransformPerspective(
            image.Handle,
            (size.Width, size.Height),
            (matrix.A, matrix.B, matrix.C, matrix.D, matrix.E, matrix.F, matrix.G, matrix.H),
            (long)resample));

    public static Image Extent(
        Image image,
        (int Width, int Height) size,
        (int Left, int Top, int Right, int Bottom) bbox,
        Resampling resample = Resampling.Bicubic) =>
        new(PillowEnvironment.Bridge.TransformExtent(
            image.Handle,
            (size.Width, size.Height),
            (bbox.Left, bbox.Top, bbox.Right, bbox.Bottom),
            (long)resample));

    public static Image Quad(
        Image image,
        (int Width, int Height) size,
        (
            int X0, int Y0, int X1, int Y1,
            int X2, int Y2, int X3, int Y3
        ) quad,
        Resampling resample = Resampling.Bicubic) =>
        new(PillowEnvironment.Bridge.TransformQuad(
            image.Handle,
            (size.Width, size.Height),
            (quad.X0, quad.Y0, quad.X1, quad.Y1, quad.X2, quad.Y2, quad.X3, quad.Y3),
            (long)resample));
}
