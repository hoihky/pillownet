namespace PillowNet;

/// <summary>Quantization methods. Mirrors <c>PIL.Image.Quantize</c>.</summary>
public enum QuantizeMethod
{
    MedianCut = 0,
    MaxCoverage = 1,
    FastOctree = 2,
    LibImageQuant = 3,
}
