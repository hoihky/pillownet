namespace PillowNet.Tests;

[Collection(PillowCollection.Name)]
public sealed class ImageMathRegressionTests
{
    [Fact]
    public void Eval_EmptyExpression_Throws()
    {
        using var image = Image.New("L", (4, 4), 0);

        Assert.Throws<ArgumentException>(() => ImageMath.Eval("   ", image));
    }
}
