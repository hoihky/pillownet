namespace PillowNet;

/// <summary>
/// Resampling filters. Mirrors <c>PIL.Image.Resampling</c>.
/// </summary>
public enum Resampling
{
    Nearest = 0,
    Lanczos = 1,
    Bilinear = 2,
    Bicubic = 3,
    Box = 4,
    Hamming = 5,
}
