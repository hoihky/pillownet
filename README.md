
> **Disclaimer:** This project is an experimental, work-in-progress prototype built with the help of "vibe coding". Things will break. Features are currently missing, and the build scripts might not work at all. Please be aware that it may not be stable enough for production use now.

# PillowNet

A C# wrapper for [Pillow](https://python-pillow.org/) (PIL) using [CSnakes](https://tonybaloney.github.io/CSnakes/) to embed Python in .NET. The API mirrors Pillow's Python design with C# naming conventions.

## Projects

| Project | Description |
|---------|-------------|
| **PillowNet.Core** | C# API (`Image`, `ImageFilter`, `ImageOps`, etc.) backed by an embedded Pillow runtime |
| **PillowNet.Cli** | Command-line tool for common image processing tasks |
| **PillowNet.Tests** | Automated regression tests (run on every test project build) |

## Prerequisites

- .NET 10 SDK
- Network access on first run (CSnakes downloads Python and installs Pillow via pip)

## Build

```bash
dotnet build
```

## Test

```bash
dotnet test
```

Building `PillowNet.Tests` also runs the test suite automatically. Disable with `-p:RunTestsAfterBuild=false` if needed.

## Documentation

- [Project introduction](docs/index.html)
- [API Reference & internals](docs/api.html)
- [Feature roadmap](docs/features.html)

Regenerate HTML from markdown (requires [MDWeb](https://github.com/hoihky/MDWeb) as a sibling repo):

```bash
./docs/build-docs.sh
```

Markdown sources live in `docs/pages/`.

## CLI Usage

```bash
# Print image metadata
dotnet run --project PillowNet.Cli -- info photo.jpg

# Convert format / color mode
dotnet run --project PillowNet.Cli -- convert input.png output.jpg --format JPEG
dotnet run --project PillowNet.Cli -- convert input.jpg output.png --mode RGBA

# Resize to exact dimensions
dotnet run --project PillowNet.Cli -- resize input.jpg output.jpg --width 800 --height 600 --resample Lanczos

# Thumbnail (preserves aspect ratio)
dotnet run --project PillowNet.Cli -- thumbnail input.jpg thumb.jpg --max-size 256

# Apply filters
dotnet run --project PillowNet.Cli -- filter input.jpg blurred.jpg --filter Blur
dotnet run --project PillowNet.Cli -- filter input.jpg soft.jpg --filter GaussianBlur --radius 4

# ImageOps enhancements
dotnet run --project PillowNet.Cli -- enhance input.jpg gray.jpg --op Grayscale
dotnet run --project PillowNet.Cli -- enhance input.jpg poster.jpg --op Posterize --bits 3

# Watermarks
dotnet run --project PillowNet.Cli -- watermark-text input.jpg out.jpg -t "Copyright" --opacity 0.4
dotnet run --project PillowNet.Cli -- watermark-image input.jpg logo.png out.jpg --anchor BottomRight
dotnet run --project PillowNet.Cli -- watermark-remove input.jpg out.jpg -l 10 -t 10 -r 200 -b 80
```

## C# API Example

```csharp
using PillowNet;

PillowEnvironment.Initialize();

using var image = Image.Open("input.jpg");
using var resized = image.Resize((800, 600), Resampling.Lanczos);
using var filtered = resized.Filter(ImageFilter.Sharpen);
filtered.Save("output.jpg");
```

## Architecture

```
┌─────────────────┐     ┌──────────────────┐     ┌─────────────┐
│  PillowNet.Cli  │────▶│  PillowNet.Core  │────▶│  CSnakes    │
│  (System.CLI)   │     │  Image, Filter…  │     │  PyObject   │
└─────────────────┘     └────────┬─────────┘     └──────┬──────┘
                                 │                      │
                                 ▼                      ▼
                        pillow_bridge.py            CPython + Pillow
```

- **Python bridge** (`pillow_bridge.py`): Typed functions for factories and filters; CSnakes source-generates C# bindings.
- **C# wrappers**: `Image` holds a `PyObject` handle and calls Pillow methods directly, matching `PIL.Image` semantics.
- **Naming**: Python `image_open` → C# `ImageOpen`; static methods use PascalCase (`Image.Open`).

## License

MIT
