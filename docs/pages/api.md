---
title: API Reference
order: 10
---

PillowNet exposes [Pillow](https://python-pillow.org/) (PIL) to .NET through [CSnakes](https://tonybaloney.github.io/CSnakes/). The public API mirrors Pillow's module layout and behavior, with C# naming conventions applied on top.

## Getting started

Every application must initialize the embedded Python runtime once before calling any API:

```csharp
using PillowNet;

PillowEnvironment.Initialize();

using var image = Image.Open("photo.jpg");
Console.WriteLine($"{image.Width}x{image.Height} {image.Mode}");
```

Call `PillowEnvironment.Shutdown()` when tearing down a long-running host (optional for CLI tools).

On first run, CSnakes downloads CPython 3.12, creates a `.venv` beside your binaries, and `pip install`s `requirements.txt` (Pillow).

---

## API overview

| C# type | Python equivalent | Role |
|---------|-------------------|------|
| `Image` | `PIL.Image.Image` | Core image type |
| `ImageFilter` | `PIL.ImageFilter` | Convolution / rank filters |
| `ImageOps` | `PIL.ImageOps` | High-level transforms |
| `ImageEnhance` | `PIL.ImageEnhance` | Brightness, contrast, color, sharpness |
| `ImageChops` | `PIL.ImageChops` | Per-pixel channel operations |
| `ImageDraw` | `PIL.ImageDraw.Draw` | In-place vector drawing |
| `ImageStat` | `PIL.ImageStat.Stat` | Pixel statistics |
| `ImageFont` | `PIL.ImageFont` | TrueType / default fonts |
| `ImageColor` | `PIL.ImageColor` | CSS-style color parsing |
| `Exif` / `ExifTag` | `PIL.Image.Exif` / `ExifTags` | EXIF read/write |
| `ImageSequence` | `PIL.ImageSequence` | Animation frames |
| `ImageTransform` | `PIL.ImageTransform` | Affine / perspective |
| `ImageMath` | `PIL.ImageMath` | Pixel expressions |
| `PixelAccess` | `Image.getpixel` / `putpixel` | Single-pixel I/O |
| `ImageCms` | `PIL.ImageCms` | ICC profile transforms |
| `Watermark` | (PillowNet) | Text/image overlay & region removal |
| `Resampling` | `PIL.Image.Resampling` | Resize / rotate filters |
| `Transpose` | `PIL.Image.Transpose` | Flip / rotate shortcuts |

### Naming conventions

| Python | C# |
|--------|-----|
| `Image.open(path)` | `Image.Open(path)` |
| `image.resize(size)` | `image.Resize(size)` |
| `ImageFilter.GaussianBlur(radius=2)` | `ImageFilter.GaussianBlur(radius: 2)` |
| `image_open` (bridge fn) | `IPillowBridge.ImageOpen` (generated) |
| `snake_case` args | `camelCase` args in generated bridge calls |

---

## Image

### Factory methods

```csharp
// Open from disk (identifies format from file contents)
using var im = Image.Open("input.png");

// Blank canvas
using var blank = Image.New("RGB", (800, 600), color: 0xFFFFFF);

// From raw pixel bytes
byte[] pixels = /* width * height * bands */;
using var raw = Image.FromBytes("RGB", (width, height), pixels);
```

### Properties

```csharp
int w = im.Width;
int h = im.Height;
(int Width, int Height) size = im.Size;
string mode = im.Mode;           // "RGB", "RGBA", "L", …
string? fmt = im.Format;         // "PNG", "JPEG", … or null
```

### Transform methods (return new `Image`)

```csharp
using var resized = im.Resize((400, 300), Resampling.Lanczos);
using var rotated = im.Rotate(45, expand: true);
using var cropped = im.Crop((10, 10, 200, 200));   // left, top, right, bottom
using var converted = im.Convert("RGBA");
using var flipped = im.Transpose(Transpose.FlipLeftRight);
using var copy = im.Copy();
using var blurred = im.Filter(ImageFilter.GaussianBlur(3));
```

### In-place methods

```csharp
im.Thumbnail((128, 128));   // preserves aspect ratio, modifies im
im.Paste(overlay, (x, y));  // paste another image at offset
im.AlphaComposite(overlay, dest: (0, 0));
```

### I/O

```csharp
im.Save("out.jpg");
im.Save("out.webp", format: "WEBP");
```

### Disposal

`Image` implements `IDisposable`. Disposing calls Pillow's `close()` and releases the native `PyObject` handle.

```csharp
using var im = Image.Open("a.png");  // preferred
```

---

## ImageFilter

Predefined and parameterized filters for `Image.Filter()`:

```csharp
im.Filter(ImageFilter.Blur);
im.Filter(ImageFilter.Sharpen);
im.Filter(ImageFilter.GaussianBlur(radius: 4));
im.Filter(ImageFilter.UnsharpMask(radius: 2, percent: 150, threshold: 3));
im.Filter(ImageFilter.Median(size: 5));
```

Available: `Blur`, `Contour`, `Detail`, `EdgeEnhance`, `EdgeEnhanceMore`, `Emboss`, `FindEdges`, `Sharpen`, `Smooth`, `SmoothMore`, `GaussianBlur`, `BoxBlur`, `UnsharpMask`, `Min`, `Max`, `Median`, `Mode`.

---

## ImageOps

Ready-made operations (each returns a new `Image`):

```csharp
ImageOps.Autocontrast(im, cutoff: 0);
ImageOps.Equalize(im);
ImageOps.Grayscale(im);
ImageOps.Invert(im);
ImageOps.Posterize(im, bits: 4);
ImageOps.Solarize(im, threshold: 128);
ImageOps.Flip(im);              // vertical
ImageOps.Mirror(im);            // horizontal
ImageOps.ExifTranspose(im);     // apply EXIF orientation
ImageOps.Expand(im, border: 20, fill: 0);
ImageOps.Colorize(im, black: 0, white: 255);
ImageOps.Fit(im, (800, 600));   // letterbox to fit
ImageOps.Pad(im, (800, 600), color: 0);
ImageOps.Contain(im, (800, 600));
ImageOps.Cover(im, (800, 600));
```

---

## ImageEnhance

Adjustment factors work like Pillow: `1.0` = original, `0.0` = minimum effect, `>1.0` = stronger.

```csharp
// One-shot helpers
ImageEnhance.Brightness(im, factor: 1.5);
ImageEnhance.Contrast(im, factor: 1.2);
ImageEnhance.Color(im, factor: 0.8);
ImageEnhance.Sharpness(im, factor: 2.0);

// Python-style enhancer objects (mirrors ImageEnhance.Brightness(im).enhance(1.5))
var enhancer = new BrightnessEnhancer(im);
using var brighter = enhancer.Enhance(1.5);
```

---

## ImageChops

Per-pixel operations on pairs of images (sizes must match):

```csharp
ImageChops.Difference(a, b);
ImageChops.Multiply(a, b);
ImageChops.Add(a, b);
ImageChops.Subtract(a, b);
ImageChops.Blend(a, b, alpha: 0.5);
ImageChops.Composite(a, b, mask);
ImageChops.Lighter(a, b);
ImageChops.Darker(a, b);
ImageChops.Screen(a, b);
ImageChops.Overlay(a, b);
```

---

## ImageDraw

Drawing modifies the target image **in place**:

```csharp
using var canvas = Image.New("RGB", (400, 200), color: 0xFFFFFF);
using (var draw = new ImageDraw(canvas))
{
    draw.Rectangle((10, 10, 200, 100), outline: 0xFF0000, width: 2);
    draw.Ellipse((50, 50, 150, 150), fill: 0x00FF00);
    draw.Line((0, 0), (400, 200), fill: 0x0000FF, width: 3);
    draw.Text((20, 160), "Hello", fill: 0x000000);
}
canvas.Save("drawing.png");
```

Optional TrueType font:

```csharp
draw.Text((10, 10), "Title", fill: 0x000000, fontPath: "fonts/MyFont.ttf", fontSize: 24);
```

---

## Phase 2 — completed

See [Feature plan](features.md).

---

## ImageFont & ImageColor

```csharp
var font = ImageFont.Truetype("fonts/MyFont.ttf", size: 24);
var (l, t, r, b) = ImageFont.GetBBox(font, "Metrics");
double width = ImageFont.GetLength(font, "Hello");

var rgb = ImageColor.GetRgb("cornflowerblue"); // [100, 149, 237]
long gray = ImageColor.GetColor("white", "L");
```

Use with `ImageDraw`:

```csharp
using var draw = new ImageDraw(canvas);
draw.Text((10, 10), "Title", font, fill: 0x000000);
```

---

## Exif & ExifTags

```csharp
var exif = image.GetExif();
if (exif.Contains(ExifTag.Orientation))
{
    var orientation = exif.Get(ExifTag.Orientation)!.As<long>();
}

exif.Set(ExifTag.Artist, "PillowNet");
image.Save("tagged.jpg", exif: exif);

foreach (var (tag, value) in exif.GetIfdItems(ExifIfd.GpsInfo))
    Console.WriteLine($"GPS 0x{tag:X4}: {value}");
```

---

## ImageSequence

```csharp
int n = ImageSequence.FrameCount(animated);
IReadOnlyList<Image> frames = ImageSequence.AllFrames(animated);
```

---

## ImageTransform

```csharp
// Scale to half size via affine matrix
using var scaled = ImageTransform.Affine(
    im, (im.Width / 2, im.Height / 2),
    matrix: (0.5, 0, 0, 0, 0.5, 0));

using var perspective = ImageTransform.Perspective(im, (400, 400),
    matrix: (1, 0.1, 0, 0, 1, 0, 0.0001, 0.0001));
```

---

## ImageMath

```csharp
// Images must be single-band (L, I, or F) — convert RGB first if needed
using var gray = imageA.Convert("L");
using var brighter = ImageMath.Eval("a + 20", gray);
using var blended = ImageMath.Eval("a + b", grayA, grayB);
```

Expressions use Pillow's `unsafe_eval` — only trusted input. RGB multi-band images are not supported directly; use `Convert("L")` or `Split()` first.

---

## PixelAccess

```csharp
PixelAccess.Load(im);
var pixel = im.GetPixel(0, 0);
PixelAccess.PutPixel(im, 0, 0, 255);
var (r, g, b) = PixelAccess.GetPixelRgb(im, 10, 10);
```

---

## ImageCms

```csharp
using var converted = ImageCms.ProfileToProfile(
    im,
    inputProfile: "sRGB.icc",
    outputProfile: "AdobeRGB.icc",
    renderingIntent: ImageCms.Intent.Perceptual);

string desc = ImageCms.GetProfileDescription("sRGB.icc");
```

---

---

## Watermark

Add semi-transparent text or image overlays, or obscure a watermark in a known rectangular region.

```csharp
// Text watermark (default: bottom-right, 50% opacity)
using var marked = Watermark.AddText(photo, "CONFIDENTIAL", new WatermarkOptions
{
    Opacity = 0.4,
    Anchor = WatermarkAnchor.Center,
    FontSize = 48,
    Color = (255, 255, 255),
    Angle = -30,
});

// Image logo watermark
using var logo = Image.Open("logo.png");
using var branded = Watermark.AddImage(photo, logo, new WatermarkOptions
{
    Opacity = 0.35,
    Scale = 0.25,
    Anchor = WatermarkAnchor.BottomRight,
    Margin = 24,
});

// Remove/obscure watermark when you know its bounding box
using var cleaned = Watermark.RemoveRegion(
    photo,
    box: (10, 10, 220, 90),
    method: WatermarkRemovalMethod.Blur,
    blurRadius: 16);
```

Removal methods:

| Method | Behavior |
|--------|----------|
| `Blur` | Gaussian blur over the region (default) |
| `Median` | Median filter — good for small text overlays |
| `Fill` | Fill with the mean color of the region |

Pillow does not perform AI inpainting; removal works best when you can specify the watermark's location.

---

## ImageStat

```csharp
var mean = ImageStat.Mean(im);       // per-channel means
var extrema = ImageStat.Extrema(im); // per-channel (min, max)
var count = ImageStat.Count(im);     // pixel count
```

---

## Complete example

```csharp
using PillowNet;

PillowEnvironment.Initialize();

using var photo = Image.Open("portrait.jpg");

// EXIF auto-rotate, fit to box, adjust tone
using var oriented = ImageOps.ExifTranspose(photo);
using var fitted = ImageOps.Fit(oriented, (1200, 800));
using var adjusted = ImageEnhance.Contrast(
    ImageEnhance.Brightness(fitted, 1.1), 1.15);
using var final = adjusted.Filter(ImageFilter.UnsharpMask(radius: 1.5));

final.Save("portrait_edited.jpg", format: "JPEG");
```

---

## Internal architecture

### Layer diagram

```
┌──────────────────────────────────────────────────────────────┐
│  Your app / PillowNet.Cli                                    │
└────────────────────────────┬─────────────────────────────────┘
                             │
┌────────────────────────────▼─────────────────────────────────┐
│  C# façade (Image, ImageFilter, ImageOps, …)                 │
│  • PascalCase API matching Pillow semantics                  │
│  • IDisposable lifetime for PyObject handles                 │
└────────────┬───────────────────────────────┬─────────────────┘
             │                               │
   CSnakes   │  IPillowBridge               │  PyObject.GetAttr / Call
   generated │  (typed bridge functions)    │  (direct Pillow methods)
             │                               │
┌────────────▼───────────────────────────────▼─────────────────┐
│  pillow_bridge.py  +  embedded Pillow (CPython heap)         │
└──────────────────────────────────────────────────────────────┘
```

### Two interop strategies

PillowNet uses **both** patterns CSnakes supports:

#### 1. Generated bridge functions (`pillow_bridge.py`)

Typed top-level Python functions are parsed at compile time. CSnakes emits `IPillowBridge` with strongly typed C# methods.

```python
def image_open(fp: str) -> Any:
    return Image.open(fp)
```

```csharp
// Generated on IPythonEnvironment:
IPillowBridge bridge = env.PillowBridge();
PyObject handle = bridge.ImageOpen("photo.jpg");
```

Use the bridge for:

- Module-level factories (`Image.open`, `Image.new`)
- Filter singletons (`ImageFilter.BLUR`)
- `ImageOps` / `ImageChops` / `ImageEnhance` helpers
- Anything that benefits from compile-time type checking on primitives (`str`, `int`, `float`, `tuple`, `bytes`)

`typing.Any` on parameters or returns tells CSnakes to pass **`PyObject`** handles without copying image pixels across the boundary.

#### 2. Direct `PyObject` method calls (`Image` instance methods)

For methods on Pillow's `Image` class, the C# wrapper holds a `PyObject` and invokes Python dynamically:

```csharp
var result = Handle.GetAttr("resize").Call(
    PyObject.From((width, height)),
    PyObject.From((long)resample));
return new Image(result);
```

This avoids writing a bridge function for every Pillow method while keeping the C# surface identical to Python (`Resize`, `Save`, `Paste`, …).

### Object lifetime

| Object | Owned by | Disposal |
|--------|----------|----------|
| `Image` | C# wrapper | `Dispose()` → `image.close()` + `PyObject.Dispose()` |
| `IImageFilter` | Static/singleton or per-call | Bridge filters are lightweight handles; factory filters allocate new PyObjects |
| Return values from transforms | Caller | Wrap in `using` — each transform returns a **new** image |

**Rules:**

1. Never use an `Image` after `Dispose()`.
2. Transforms return new instances; the source image is unchanged (except `Thumbnail`, `Paste`, `AlphaComposite`, and `ImageDraw`).
3. Do not share one `Image` across threads without external synchronization (Python GIL is managed by CSnakes per call).

### Runtime bootstrap (`PillowEnvironment`)

```csharp
builder.Services
    .WithPython()
    .WithHome(AppContext.BaseDirectory)      // pillow_bridge.py lives here
    .WithVirtualEnvironment(".venv")
    .WithPipInstaller("requirements.txt")
    .FromRedistributable();                  // auto-download CPython
```

`IPillowBridge` is resolved once and cached. Initialization is idempotent.

### Type mapping

| Python (annotations) | C# |
|---------------------|-----|
| `str` | `string` |
| `int` | `long` |
| `float` | `double` |
| `bool` | `bool` |
| `bytes` | `byte[]` |
| `tuple[int, int]` | `(long, long)` or `(int, int)` |
| `Any` / Pillow objects | `PyObject` |
| `None` optional | `T?` |

### Error handling

Python exceptions become `CSnakes.Runtime.PythonInvocationException` with the original message in `InnerException`. Typical cases: unsupported mode, size mismatch for `ImageChops`, corrupt files.

---

## Feature roadmap

See [Feature plan](features.md) for the full Pillow coverage plan and what was added in each phase.

### Currently wrapped

- **Image**: open, new, frombytes, save (with EXIF), resize, thumbnail, convert, filter, rotate, crop, copy, transpose, paste, alpha composite, split, merge, getexif, getpixel, putpixel, load
- **ImageFilter**: all predefined + parameterized filters
- **ImageOps**: full Phase 2 set
- **ImageEnhance**, **ImageChops**, **ImageDraw**, **ImageStat**
- **ImageFont**, **ImageColor**, **Exif** / **ExifTags**
- **ImageSequence**, **ImageTransform**, **ImageMath**, **PixelAccess**, **ImageCms**, **Watermark**

### Planned (Phase 4)

| Module | Notes |
|--------|-------|
| `ImageMorph` | Morphological ops |
| `ImageQt` / `ImageTk` | Out of scope (GUI) |
| `ImageGrab` | Out of scope (screen capture) |

---

## CLI quick reference

```bash
dotnet run --project PillowNet.Cli -- info IMAGE
dotnet run --project PillowNet.Cli -- convert IN OUT [--format FMT] [--mode MODE]
dotnet run --project PillowNet.Cli -- resize IN OUT -w W -h H [--resample Lanczos]
dotnet run --project PillowNet.Cli -- thumbnail IN OUT [--max-size 128]
dotnet run --project PillowNet.Cli -- filter IN OUT -f GaussianBlur [--radius 3]
dotnet run --project PillowNet.Cli -- enhance IN OUT -o Grayscale
dotnet run --project PillowNet.Cli -- adjust IN OUT --brightness 1.2 --contrast 1.1
dotnet run --project PillowNet.Cli -- flip IN OUT [--horizontal]
dotnet run --project PillowNet.Cli -- composite BASE OVERLAY OUT [--alpha 0.5]
dotnet run --project PillowNet.Cli -- draw IN OUT --text "Hello" --x 10 --y 10
dotnet run --project PillowNet.Cli -- exif IMAGE
dotnet run --project PillowNet.Cli -- frames ANIMATED_GIF ./frames/
dotnet run --project PillowNet.Cli -- math A.PNG OUT.PNG -e "a * 1.5"
dotnet run --project PillowNet.Cli -- pixel IMAGE --x 0 --y 0
dotnet run --project PillowNet.Cli -- pixel IMAGE --x 0 --y 0 --set 255 -o out.png
dotnet run --project PillowNet.Cli -- watermark-text IN OUT -t "Copyright" --opacity 0.4
dotnet run --project PillowNet.Cli -- watermark-image IN LOGO.PNG OUT --opacity 0.3 --anchor BottomRight
dotnet run --project PillowNet.Cli -- watermark-remove IN OUT -l 10 -t 10 -r 200 -b 80 --method Blur
```
