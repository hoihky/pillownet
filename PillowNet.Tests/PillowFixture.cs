namespace PillowNet.Tests;

public sealed class PillowFixture : IDisposable
{
    public PillowFixture() => PillowEnvironment.Initialize();

    public void Dispose() => PillowEnvironment.Shutdown();
}

[CollectionDefinition(Name)]
public sealed class PillowCollection : ICollectionFixture<PillowFixture>
{
    public const string Name = "Pillow";
}
