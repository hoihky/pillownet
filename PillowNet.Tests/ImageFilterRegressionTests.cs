namespace PillowNet.Tests;

[Collection(PillowCollection.Name)]
public sealed class ImageFilterRegressionTests
{
    [Fact]
    public void Blur_CanBeAppliedTwiceWithoutLeaking()
    {
        using var image = Image.New("RGB", (12, 12), 0);

        using var first = image.Filter(ImageFilter.Blur);
        using var second = first.Filter(ImageFilter.Blur);

        Assert.Equal(12, second.Width);
        Assert.Equal(12, second.Height);
    }
}
