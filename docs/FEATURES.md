# PillowNet Feature Plan

This document tracks Pillow module coverage, implementation phases, and research notes.

## Pillow module inventory

| Python module | Purpose | PillowNet status |
|---------------|---------|------------------|
| `PIL.Image` | Core image type | **Wrapped** (core + Phase 4 extensions) |
| `PIL.ImageFilter` | Convolution filters | **Wrapped** |
| `PIL.ImageOps` | Common transforms | **Wrapped** (extended set) |
| `PIL.ImageEnhance` | Tone adjustments | **Wrapped** |
| `PIL.ImageChops` | Channel math | **Wrapped** (common ops) |
| `PIL.ImageDraw` | Vector drawing | **Wrapped** (shapes + polygon) |
| `PIL.ImageStat` | Statistics | **Wrapped** |
| `PIL.ImageFont` | Font objects | **Wrapped** |
| `PIL.ImageColor` | Color parsing | **Wrapped** |
| `PIL.ImageCms` | ICC / color management | **Wrapped** (basic) |
| `PIL.ImageMath` | Pixel expressions | **Wrapped** |
| `PIL.ImageMorph` | Morphology | **Wrapped** (named operators) |
| `PIL.ImageSequence` | Animation frames | **Wrapped** |
| `PIL.ExifTags` | EXIF constants | **Wrapped** |
| `PIL.ImageTransform` | Affine / perspective | **Wrapped** |
| `PIL.ImageQt` / `ImageTk` | GUI | Out of scope |
| `PIL.ImageGrab` | Screen capture | Out of scope |
| `PIL.ImageWin` | Windows DIB | Out of scope |
| `PIL.PdfImagePlugin` etc. | Format plugins | Via `Image.Open` / `Save` |

## Phase 1–3 (completed)

See git history and [API.md](API.md) for the initial through watermark releases.

## Phase 4 (in progress)

### Implemented

**Image**
- `GetBBox`, `ToBytes`, `GetBands`, `GetChannel`, `Quantize`, `Seek`, `Tell`
- `New` with RGB tuple / object color (fixes int-only limitation)

**ImageDraw**
- `Polygon`

**ImageMorph**
- `Apply`, `Match` with built-in operator names (`erosion4`, `dilation8`, `edge`, …)

**Exif**
- `GetIfdStrings` (managed read without leaking `PyObject`)

**Bug fixes**
- `ImageColor.GetRgb` / `GetColor` dispose bridge handles
- CLI `enhance --op Posterize` validates bits 1–8

### Planned next (high value)

| Feature | Pillow API | Use case |
|---------|------------|----------|
| Save options | `Image.save(..., quality=, optimize=)` | JPEG/WebP output control |
| Point transform | `Image.point()` | Thresholding, curves |
| Histogram | `Image.histogram()` | Analysis, auto-levels |
| Palette mode | `getpalette` / `putpalette` | GIF/PNG-P workflows |
| Bulk pixels | `getdata` / `putdata` | Fast buffer I/O |
| Draw extras | `arc`, `chord`, `pieslice`, `multiline_text` | Rich annotations |
| ImageChops extras | `constant`, `offset`, `logical` ops | Compositing |
| ImageOps extras | `scale`, `deform` | Thumbnails, warping |
| ImageMorph LUT | `LutBuilder`, custom patterns | Custom morphology |
| EXIF GPS | `ExifTags` GPS enum coverage | Location metadata |
| Features probe | `PIL.features.check` | Runtime capability checks |

### Lower priority / out of scope

- `ImageGrab`, `ImageQt`, `ImageTk` (platform/GUI specific)
- NumPy `fromarray` / `Image.fromarray` (add when buffer API exists)
- `Image.show()` (requires GUI viewer)

## Research notes (Pillow 12.x)

### Most-requested Image methods not yet wrapped

1. **`point(lut, mode)`** — per-pixel lookup; essential for thresholding and tone curves.
2. **`histogram()`** — returns 256×bands values; useful with `ImageStat` extensions.
3. **`save` keyword args** — `quality`, `subsampling`, `lossless`, `duration` (GIF), `append_images`.
4. **`getdata` / `putdata`** — iterator over flattened pixels; fastest path for bulk edits.
5. **`reduce(factor)`** — fast downscale by integer factor.
6. **`getpalette` / `putpalette`** — palette manipulation for `P` mode.
7. **`transform` generic** — already partially covered by `ImageTransform`; generic `Image.transform` still missing.
8. **`alpha_composite` / `paste` mask** — `paste` supports mask kwarg; not exposed yet.

### ImageDraw gaps

- `multiline_text`, `textbbox`, `textlength` on draw context
- `arc`, `chord`, `pieslice`, `regular_polygon`

### ImageFilter gaps

- Custom `Kernel` / `RankFilter` construction
- `ADD`, `SUBTRACT` etc. module-level constants (less common)

### ImageMorph gaps

- `LutBuilder` for custom patterns
- `load_lut` / `save_lut` file I/O

### Animation gaps

- `ImageSequence.Iterator` lazy iteration (currently `AllFrames` copies all)
- `save_all` / `append_images` for building GIFs from frames

## Design principles

1. **Mirror Pillow semantics** — same defaults, same in-place vs copy behavior.
2. **C# conventions** — PascalCase types/methods, `IDisposable` for images and transient handles.
3. **Bridge vs PyObject** — factories in `pillow_bridge.py`; instance methods on `Image` via `PyObject`.
4. **Minimal scope** — wrap what Pillow provides; don't reimplement algorithms in C#.
5. **No handle leaks** — dispose `PyObject` temporaries; prefer managed return types where practical.
