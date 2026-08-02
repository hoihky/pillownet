using CSnakes.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PillowNet;

/// <summary>
/// Bootstraps the embedded Python runtime and Pillow for PillowNet.
/// </summary>
public static class PillowEnvironment
{
    private static readonly object InitLock = new();
    private static IHost? _host;
    private static IPillowBridge? _bridge;

    internal static IPillowBridge Bridge =>
        _bridge ?? throw new InvalidOperationException(
            "PillowNet is not initialized. Call PillowEnvironment.Initialize() first.");

    /// <summary>
    /// Initializes Python, installs Pillow if needed, and prepares the bridge module.
    /// </summary>
    public static void Initialize(string? homeDirectory = null)
    {
        lock (InitLock)
        {
            if (_bridge is not null)
            {
                return;
            }

            homeDirectory = ResolveHomeDirectory(homeDirectory);
            var venvPath = Path.Combine(homeDirectory, ".venv");
            var requirementsPath = Path.Combine(homeDirectory, "requirements.txt");

            var builder = Host.CreateApplicationBuilder();
            builder.Services
                .WithPython()
                .WithHome(homeDirectory)
                .WithVirtualEnvironment(venvPath)
                .WithPipInstaller(requirementsPath)
                .FromRedistributable();

            IHost? host = null;
            try
            {
                host = builder.Build();
                var environment = host.Services.GetRequiredService<IPythonEnvironment>();
                _bridge = environment.PillowBridge();
                _host = host;
            }
            catch
            {
                host?.Dispose();
                _host = null;
                _bridge = null;
                throw;
            }
        }
    }

    /// <summary>
    /// Shuts down the embedded Python runtime.
    /// </summary>
    public static void Shutdown()
    {
        lock (InitLock)
        {
            _host?.Dispose();
            _host = null;
            _bridge = null;
        }
    }

    private static string ResolveHomeDirectory(string? homeDirectory)
    {
        if (homeDirectory is not null)
        {
            return homeDirectory;
        }

        var entryDir = AppContext.BaseDirectory;
        if (HasRuntimeAssets(entryDir))
        {
            return entryDir;
        }

        var coreDir = Path.GetDirectoryName(typeof(PillowEnvironment).Assembly.Location)!;
        if (HasRuntimeAssets(coreDir))
        {
            return coreDir;
        }

        return entryDir;
    }

    private static bool HasRuntimeAssets(string directory) =>
        File.Exists(Path.Combine(directory, "pillow_bridge.py")) &&
        File.Exists(Path.Combine(directory, "requirements.txt"));
}
