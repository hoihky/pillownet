namespace PillowNet.Tests;

[Collection(PillowCollection.Name)]
public sealed class ExifRegressionTests
{
    [Fact]
    public void Get_MissingTag_ReturnsNull()
    {
        using var image = Image.New("RGB", (4, 4), 0);
        using var exif = image.GetExif();

        Assert.Null(exif.Get(ExifTag.Artist));
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        using var image = Image.New("RGB", (4, 4), 0);
        var exif = image.GetExif();

        var exception = Record.Exception(() => exif.Dispose());

        Assert.Null(exception);
    }
}
