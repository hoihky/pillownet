using CSnakes.Runtime.Python;

namespace PillowNet.Tests;

[Collection(PillowCollection.Name)]
public sealed class ImageRegressionTests
{
    [Fact]
    public void Format_NewImage_ReturnsNull()
    {
        using var image = Image.New("RGB", (4, 4), 0);

        Assert.Null(image.Format);
    }

    [Fact]
    public void DisposedImage_ThrowsOnPropertyAccess()
    {
        var image = Image.New("RGB", (2, 2), 0);
        image.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = image.Width);
    }

    [Fact]
    public void Split_RgbImage_ReturnsThreeBands()
    {
        using var image = Image.New("RGB", (8, 8), 0);
        var bands = image.Split();

        Assert.Equal(3, bands.Count);
        foreach (var band in bands)
        {
            band.Dispose();
        }
    }

    [Fact]
    public void PutPixel_GetPixel_RoundtripsOnGrayscale()
    {
        using var image = Image.New("L", (4, 4), 0);

        image.PutPixel(1, 2, 200);
        var pixel = PixelAccess.GetPixelGray(image, 1, 2);

        Assert.Equal(200, pixel);
    }
}
