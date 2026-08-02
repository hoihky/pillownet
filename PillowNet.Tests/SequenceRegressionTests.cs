namespace PillowNet.Tests;

[Collection(PillowCollection.Name)]
public sealed class SequenceRegressionTests
{
    [Fact]
    public void FrameCount_StaticImage_LeavesImageReadable()
    {
        using var image = Image.New("RGB", (16, 8), 0);

        var frameCount = ImageSequence.FrameCount(image);

        Assert.Equal(1, frameCount);
        Assert.Equal(16, image.Width);
        Assert.Equal(8, image.Height);
    }
}
