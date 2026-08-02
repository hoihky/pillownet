namespace PillowNet.Tests;

[Collection(PillowCollection.Name)]
public sealed class WatermarkRegressionTests
{
    [Fact]
    public void GetTextBBox_ScalesWithFontSize()
    {
        var small = ImageFont.GetTextBBox("Watermark", fontPath: null, fontSize: 12);
        var large = ImageFont.GetTextBBox("Watermark", fontPath: null, fontSize: 72);

        var smallHeight = small.Bottom - small.Top;
        var largeHeight = large.Bottom - large.Top;

        Assert.True(largeHeight > smallHeight);
    }

    [Fact]
    public void AddText_WithLargeFont_ProducesValidImage()
    {
        using var image = Image.New("RGB", (200, 100), 0);
        using var result = Watermark.AddText(
            image,
            "TEST",
            new WatermarkOptions { FontSize = 48, Opacity = 0.8 });

        Assert.Equal(200, result.Width);
        Assert.Equal(100, result.Height);
        Assert.Equal("RGB", result.Mode);
    }
}
