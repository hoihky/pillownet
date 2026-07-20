# PillowNet Feature Plan

This document tracks Pillow module coverage and implementation phases.

## Pillow module inventory

| Python module | Purpose | PillowNet status |
|---------------|---------|------------------|
| `PIL.Image` | Core image type | **Wrapped** (core methods) |
| `PIL.ImageFilter` | Convolution filters | **Wrapped** |
| `PIL.ImageOps` | Common transforms | **Wrapped** (extended set) |
| `PIL.ImageEnhance` | Tone adjustments | **Wrapped** (Phase 2) |
| `PIL.ImageChops` | Channel math | **Wrapped** (common ops) |
| `PIL.ImageDraw` | Vector drawing | **Wrapped** (basic shapes) |
| `PIL.ImageStat` | Statistics | **Wrapped** (Phase 2) |
| `PIL.ImageFont` | Font objects | **Wrapped** (Phase 3) |
| `PIL.ImageColor` | Color parsing | **Wrapped** (Phase 3) |
| `PIL.ImageCms` | ICC / color management | **Wrapped** (Phase 3) |
| `PIL.ImageMath` | Pixel expressions | **Wrapped** (Phase 3) |
| `PIL.ImageMorph` | Morphology | Planned |
| `PIL.ImageSequence` | Animation frames | **Wrapped** (Phase 3) |
| `PIL.ExifTags` | EXIF constants | **Wrapped** (Phase 3) |
| `PIL.ImageTransform` | Affine / perspective | **Wrapped** (Phase 3) |
| `PIL.ImageQt` / `ImageTk` | GUI | Out of scope |
| `PIL.ImageGrab` | Screen capture | Out of scope |

## Phase 1 (initial release)

- `Image`: Open, New, FromBytes, Save, Resize, Thumbnail, Convert, Filter, Rotate, Crop, Copy, Transpose
- `ImageFilter`: all predefined + parameterized filters
- `ImageOps`: autocontrast, equalize, grayscale, invert, posterize, solarize
- CLI: convert, resize, thumbnail, filter, enhance, info

## Phase 2 (completed)

**Goal:** Cover the most common Pillow workflows missing from Phase 1.

### ImageEnhance
- `Brightness`, `Contrast`, `Color`, `Sharpness` enhancers with `Enhance(factor)`

### ImageOps (extended)
- `Flip`, `Mirror`, `ExifTranspose`, `Expand`, `Colorize`
- `Fit`, `Pad`, `Contain`, `Cover` (aspect-ratio helpers)

### ImageChops
- `Difference`, `Multiply`, `Add`, `Subtract`, `Blend`, `Composite`
- `Lighter`, `Darker`, `Screen`, `Overlay`

### Image methods
- `Paste`, `AlphaComposite`, `Split`

### ImageDraw
- `Rectangle`, `Ellipse`, `Line`, `Text` (optional TTF path)

### ImageStat
- `Mean`, `Extrema`, `Count`

### CLI
- `adjust` (brightness/contrast/sharpness/color)
- `flip` (horizontal/vertical)
- `composite` (blend two images)
- `draw` (text overlay)

## Phase 3 (implemented)

- `ImageFont` + `ImageColor` for rich text rendering
- `Image.getexif()` / `ExifTags` for metadata read/write
- `ImageSequence` for animated GIF/WebP frames
- `ImageTransform` (Affine, Perspective, Extent, Quad)
- `ImageMath` eval expressions
- `PixelAccess` for fast pixel read/write
- `ImageCms` profile transforms

### CLI
- `exif`, `frames`, `math`, `pixel`
- `watermark-text`, `watermark-image`, `watermark-remove`

## Phase 4 (future)

- `ImageMorph` morphology operations
- Extended `ExifTags` GPS/EXIF enum coverage
- `PixelAccess` bulk buffer API

## Design principles

1. **Mirror Pillow semantics** — same defaults, same in-place vs copy behavior.
2. **C# conventions** — PascalCase types/methods, `IDisposable` for images.
3. **Bridge vs PyObject** — factories in `pillow_bridge.py`; instance methods on `Image` via `PyObject`.
4. **Minimal scope** — wrap what Pillow provides; don't reimplement algorithms in C#.
