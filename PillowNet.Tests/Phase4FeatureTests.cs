namespace PillowNet.Tests;

[Collection(PillowCollection.Name)]
public sealed class Phase4FeatureTests
{
    [Fact]
    public void New_WithRgbTuple_CreatesColoredImage()
    {
        using var image = Image.New("RGB", (4, 4), (255, 0, 0));

        Assert.Equal("RGB", image.Mode);
        Assert.Equal((255, 0, 0), image.GetPixelRgb(0, 0));
    }

    [Fact]
    public void GetBands_Rgb_ReturnsThreeBands()
    {
        using var image = Image.New("RGB", (2, 2), 0);

        Assert.Equal(["R", "G", "B"], image.GetBands());
    }

    [Fact]
    public void GetBBox_OnFilledImage_ReturnsFullBounds()
    {
        using var image = Image.New("RGB", (10, 8), (255, 255, 255));

        var box = image.GetBBox();

        Assert.Equal((0, 0, 10, 8), box);
    }

    [Fact]
    public void ToBytes_ReturnsPixelData()
    {
        using var image = Image.New("L", (2, 2), 128);

        var bytes = image.ToBytes();

        Assert.Equal(4, bytes.Length);
    }

    [Fact]
    public void Quantize_Rgb_ReturnsPaletteImage()
    {
        using var image = Image.New("RGB", (16, 16), (200, 100, 50));
        using var quantized = image.Quantize(colors: 16);

        Assert.Equal("P", quantized.Mode);
    }

    [Fact]
    public void GetChannel_Red_ReturnsSingleBand()
    {
        using var image = Image.New("RGB", (4, 4), (10, 20, 30));
        using var red = image.GetChannel(0);

        Assert.Equal("L", red.Mode);
        Assert.Equal(10, red.GetPixelGray(0, 0));
    }

    [Fact]
    public void Polygon_Draw_ChangesImage()
    {
        using var image = Image.New("RGB", (20, 20), 0);
        var before = ImageStat.Mean(image).ToArray();

        using (var draw = new ImageDraw(image))
        {
            draw.Polygon(
                [(2, 2), (18, 2), (10, 18)],
                fill: (255, 255, 255));
        }

        var after = ImageStat.Mean(image).ToArray();

        Assert.NotEqual(before, after);
    }

    [Fact]
    public void ImageMorph_Erosion_ChangesPixels()
    {
        using var image = Image.New("L", (12, 12), 0);
        for (var x = 3; x < 9; x++)
        {
            for (var y = 3; y < 9; y++)
            {
                image.PutPixel(x, y, 255);
            }
        }

        var (changed, result) = ImageMorph.Apply(image, ImageMorph.Operators.Erosion4);
        using (result)
        {
            Assert.True(changed > 0);
            Assert.Equal("L", result.Mode);
        }
    }

    [Fact]
    public void ImageColor_GetRgb_ParsesNamedColor()
    {
        var rgb = ImageColor.GetRgb("white");

        Assert.Equal([255, 255, 255], rgb);
    }
}
