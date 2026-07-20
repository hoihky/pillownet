namespace PillowNet;

/// <summary>Common EXIF tag IDs. Mirrors <c>PIL.ExifTags.Base</c>.</summary>
public enum ExifTag
{
    Orientation = 0x0112,
    ImageWidth = 0x0100,
    ImageLength = 0x0101,
    Make = 0x010F,
    Model = 0x0110,
    Software = 0x0131,
    DateTime = 0x0132,
    Artist = 0x013B,
    XResolution = 0x011A,
    YResolution = 0x011B,
    ResolutionUnit = 0x0128,
    ColorSpace = 0xA001,
    ExifOffset = 0x8769,
    GpsInfo = 0x8825,
}

/// <summary>EXIF IFD groups. Mirrors <c>PIL.ExifTags.IFD</c>.</summary>
public enum ExifIfd
{
    Exif = 0x8769,
    GpsInfo = 0x8825,
    MakerNote = 0x927C,
    Interop = 0xA005,
    Ifd1 = -1,
}
