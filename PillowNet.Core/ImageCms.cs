namespace PillowNet;

/// <summary>ICC color management. Mirrors <c>PIL.ImageCms</c>.</summary>
public static class ImageCms
{
    /// <summary>Rendering intents. Mirrors <c>ImageCms.Intent</c>.</summary>
    public enum Intent
    {
        Perceptual = 0,
        RelativeColorimetric = 1,
        Saturation = 2,
        AbsoluteColorimetric = 3,
    }

    /// <summary>
    /// Converts an image between ICC profiles.
    /// Mirrors <c>ImageCms.profileToProfile</c>.
    /// </summary>
    public static Image ProfileToProfile(
        Image image,
        string inputProfile,
        string outputProfile,
        Intent renderingIntent = Intent.Perceptual,
        string? outputMode = null) =>
        new(PillowEnvironment.Bridge.CmsProfileToProfile(
            image.Handle,
            inputProfile,
            outputProfile,
            (long)renderingIntent,
            outputMode));

    public static string GetProfileDescription(string profilePath) =>
        PillowEnvironment.Bridge.CmsGetProfileDescription(profilePath);
}
