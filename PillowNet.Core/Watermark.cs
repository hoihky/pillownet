namespace PillowNet;

/// <summary>Placement anchor for image watermarks.</summary>
public enum WatermarkAnchor
{
    TopLeft,
    TopCenter,
    TopRight,
    CenterLeft,
    Center,
    CenterRight,
    BottomLeft,
    BottomCenter,
    BottomRight,
}

/// <summary>Strategy for removing a watermark from a rectangular region.</summary>
public enum WatermarkRemovalMethod
{
    /// <summary>Gaussian blur over the region.</summary>
    Blur,

    /// <summary>Median filter over the region.</summary>
    Median,

    /// <summary>Fill the region with the mean color of its pixels.</summary>
    Fill,
}

/// <summary>Options for adding a watermark.</summary>
public sealed class WatermarkOptions
{
    public (int X, int Y)? Position { get; init; }

    public WatermarkAnchor Anchor { get; init; } = WatermarkAnchor.BottomRight;

    public int Margin { get; init; } = 16;

    public double Opacity { get; init; } = 0.5;

    public double Scale { get; init; } = 1.0;

    public double Angle { get; init; }

    public string? FontPath { get; init; }

    public int FontSize { get; init; } = 48;

    public (int R, int G, int B) Color { get; init; } = (255, 255, 255);
}

/// <summary>Add and remove watermarks. Built on Pillow alpha compositing and filters.</summary>
public static class Watermark
{
    /// <summary>Overlays semi-transparent text. Returns a new image.</summary>
    public static Image AddText(Image image, string text, WatermarkOptions? options = null)
    {
        options ??= new WatermarkOptions();
        var (x, y) = ResolveTextPosition(image, text, options);
        return new(PillowEnvironment.Bridge.WatermarkAddText(
            image.Handle,
            text,
            x,
            y,
            options.Opacity,
            options.Color.R,
            options.Color.G,
            options.Color.B,
            options.FontPath,
            options.FontSize,
            options.Angle));
    }

    /// <summary>Overlays a semi-transparent image watermark. Returns a new image.</summary>
    public static Image AddImage(Image image, Image watermark, WatermarkOptions? options = null)
    {
        options ??= new WatermarkOptions();
        var (x, y) = ResolveImagePosition(image, watermark, options);
        return new(PillowEnvironment.Bridge.WatermarkAddImage(
            image.Handle,
            watermark.Handle,
            x,
            y,
            options.Opacity,
            options.Scale));
    }

    /// <summary>
    /// Reduces or obscures a watermark inside a rectangular region.
    /// Returns a new image. Use when the watermark location is known.
    /// </summary>
    public static Image RemoveRegion(
        Image image,
        (int Left, int Top, int Right, int Bottom) box,
        WatermarkRemovalMethod method = WatermarkRemovalMethod.Blur,
        double blurRadius = 12.0) =>
        new(PillowEnvironment.Bridge.WatermarkRemoveRegion(
            image.Handle,
            box.Left,
            box.Top,
            box.Right,
            box.Bottom,
            method.ToString().ToLowerInvariant(),
            blurRadius));

    private static (int X, int Y) ResolveTextPosition(Image image, string text, WatermarkOptions options)
    {
        if (options.Position is { } pos)
        {
            return pos;
        }

        var box = ImageFont.GetTextBBox(
            text,
            options.FontPath,
            options.FontSize);

        var textWidth = box.Right - box.Left;
        var textHeight = box.Bottom - box.Top;

        return ResolveAnchoredPosition(
            image.Width,
            image.Height,
            textWidth,
            textHeight,
            options.Anchor,
            options.Margin);
    }

    private static (int X, int Y) ResolveImagePosition(
        Image image,
        Image watermark,
        WatermarkOptions options)
    {
        var markWidth = (int)(watermark.Width * options.Scale);
        var markHeight = (int)(watermark.Height * options.Scale);

        if (options.Position is { } pos)
        {
            return pos;
        }

        return ResolveAnchoredPosition(
            image.Width,
            image.Height,
            markWidth,
            markHeight,
            options.Anchor,
            options.Margin);
    }

    private static (int X, int Y) ResolveAnchoredPosition(
        int canvasWidth,
        int canvasHeight,
        int markWidth,
        int markHeight,
        WatermarkAnchor anchor,
        int margin)
    {
        return anchor switch
        {
            WatermarkAnchor.TopLeft => (margin, margin),
            WatermarkAnchor.TopCenter => ((canvasWidth - markWidth) / 2, margin),
            WatermarkAnchor.TopRight => (canvasWidth - markWidth - margin, margin),
            WatermarkAnchor.CenterLeft => (margin, (canvasHeight - markHeight) / 2),
            WatermarkAnchor.Center => ((canvasWidth - markWidth) / 2, (canvasHeight - markHeight) / 2),
            WatermarkAnchor.CenterRight => (canvasWidth - markWidth - margin, (canvasHeight - markHeight) / 2),
            WatermarkAnchor.BottomLeft => (margin, canvasHeight - markHeight - margin),
            WatermarkAnchor.BottomCenter => ((canvasWidth - markWidth) / 2, canvasHeight - markHeight - margin),
            WatermarkAnchor.BottomRight => (canvasWidth - markWidth - margin, canvasHeight - markHeight - margin),
            _ => (margin, margin),
        };
    }
}
