using CSnakes.Runtime.Python;

namespace PillowNet;

/// <summary>Pixel expression evaluation. Mirrors <c>PIL.ImageMath</c>.</summary>
public static class ImageMath
{
    /// <summary>
    /// Evaluates a Pillow image expression. Mirrors <c>ImageMath.unsafe_eval</c>.
    /// Use variable names <c>a</c>/<c>image</c> and optionally <c>b</c> in the expression.
    /// </summary>
    public static Image Eval(string expression, Image? a = null, Image? b = null)
    {
        PyObject? aHandle = a?.Handle;
        PyObject? bHandle = b?.Handle;
        var result = PillowEnvironment.Bridge.ImagemathEval(
            expression,
            aHandle,
            bHandle);

        return new Image(result);
    }
}
