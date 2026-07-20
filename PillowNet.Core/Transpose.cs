namespace PillowNet;

/// <summary>
/// Transpose methods. Mirrors <c>PIL.Image.Transpose</c>.
/// </summary>
public enum Transpose
{
    FlipLeftRight = 0,
    FlipTopBottom = 1,
    Rotate90 = 2,
    Rotate180 = 3,
    Rotate270 = 4,
    Transpose = 5,
    Transverse = 6,
}
