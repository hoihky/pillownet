namespace PillowNet.Tests;

[Collection(PillowCollection.Name)]
public sealed class DrawRegressionTests
{
    [Fact]
    public void Text_WithRgbTuple_ChangesImage()
    {
        using var image = Image.New("RGB", (40, 20), 0);
        var before = ImageStat.Mean(image).ToArray();

        using (var draw = new ImageDraw(image))
        {
            draw.Text((4, 4), "Hi", fill: (255, 255, 255));
        }

        var after = ImageStat.Mean(image).ToArray();

        Assert.NotEqual(before, after);
    }

    [Fact]
    public void Constructor_OnDisposedImage_Throws()
    {
        var image = Image.New("RGB", (8, 8), 0);
        image.Dispose();

        Assert.Throws<ObjectDisposedException>(() => new ImageDraw(image));
    }
}
