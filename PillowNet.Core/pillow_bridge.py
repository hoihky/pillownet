"""Typed bridge between CSnakes and Pillow (PIL).

Functions use ``typing.Any`` for Pillow Image/Filter objects so CSnakes
marshals them as PyObject handles that stay in the Python heap.
"""

from __future__ import annotations

from typing import Any

from PIL import (
    Image,
    ImageChops,
    ImageCms,
    ImageColor,
    ImageDraw,
    ImageEnhance,
    ImageFilter,
    ImageFont,
    ImageMath,
    ImageOps,
    ImageSequence,
    ImageStat,
    ImageTransform,
)


# --- Image factories ---


def image_open(fp: str) -> Any:
    return Image.open(fp)


def image_new(mode: str, size: tuple[int, int], color: int = 0) -> Any:
    return Image.new(mode, size, color)


def image_frombytes(
    mode: str,
    size: tuple[int, int],
    data: bytes,
) -> Any:
    return Image.frombytes(mode, size, data)


def image_close(image: Any) -> None:
    image.close()


def image_merge(mode: str, bands: list[Any]) -> Any:
    return Image.merge(mode, bands)


# --- ImageFilter ---


def filter_blur() -> Any:
    return ImageFilter.BLUR


def filter_contour() -> Any:
    return ImageFilter.CONTOUR


def filter_detail() -> Any:
    return ImageFilter.DETAIL


def filter_edge_enhance() -> Any:
    return ImageFilter.EDGE_ENHANCE


def filter_edge_enhance_more() -> Any:
    return ImageFilter.EDGE_ENHANCE_MORE


def filter_emboss() -> Any:
    return ImageFilter.EMBOSS


def filter_find_edges() -> Any:
    return ImageFilter.FIND_EDGES


def filter_sharpen() -> Any:
    return ImageFilter.SHARPEN


def filter_smooth() -> Any:
    return ImageFilter.SMOOTH


def filter_smooth_more() -> Any:
    return ImageFilter.SMOOTH_MORE


def filter_gaussian_blur(radius: float = 2.0) -> Any:
    return ImageFilter.GaussianBlur(radius)


def filter_box_blur(radius: float) -> Any:
    return ImageFilter.BoxBlur(radius)


def filter_unsharp_mask(
    radius: float = 2.0,
    percent: int = 150,
    threshold: int = 3,
) -> Any:
    return ImageFilter.UnsharpMask(radius, percent, threshold)


def filter_min(size: int = 3) -> Any:
    return ImageFilter.MinFilter(size)


def filter_max(size: int = 3) -> Any:
    return ImageFilter.MaxFilter(size)


def filter_median(size: int = 3) -> Any:
    return ImageFilter.MedianFilter(size)


def filter_mode_filter(size: int = 3) -> Any:
    return ImageFilter.ModeFilter(size)


# --- ImageOps ---


def image_ops_autocontrast(image: Any, cutoff: float = 0) -> Any:
    return ImageOps.autocontrast(image, cutoff)


def image_ops_equalize(image: Any) -> Any:
    return ImageOps.equalize(image)


def image_ops_grayscale(image: Any) -> Any:
    return ImageOps.grayscale(image)


def image_ops_invert(image: Any) -> Any:
    return ImageOps.invert(image)


def image_ops_posterize(image: Any, bits: int) -> Any:
    return ImageOps.posterize(image, bits)


def image_ops_solarize(image: Any, threshold: int = 128) -> Any:
    return ImageOps.solarize(image, threshold)


def image_ops_flip(image: Any) -> Any:
    return ImageOps.flip(image)


def image_ops_mirror(image: Any) -> Any:
    return ImageOps.mirror(image)


def image_ops_exif_transpose(image: Any) -> Any:
    return ImageOps.exif_transpose(image)


def image_ops_expand(image: Any, border: int, fill: int = 0) -> Any:
    return ImageOps.expand(image, border=border, fill=fill)


def image_ops_colorize(image: Any, black: int, white: int) -> Any:
    return ImageOps.colorize(image, black, white)


def image_ops_fit(
    image: Any,
    size: tuple[int, int],
    method: int = 3,
    color: int = 0,
) -> Any:
    return ImageOps.fit(image, size, method=method, color=color)


def image_ops_pad(
    image: Any,
    size: tuple[int, int],
    method: int = 3,
    color: int = 0,
) -> Any:
    return ImageOps.pad(image, size, method=method, color=color)


def image_ops_contain(
    image: Any,
    size: tuple[int, int],
    method: int = 3,
) -> Any:
    return ImageOps.contain(image, size, method=method)


def image_ops_cover(
    image: Any,
    size: tuple[int, int],
    method: int = 3,
) -> Any:
    return ImageOps.cover(image, size, method=method)


# --- ImageEnhance ---


def enhance_brightness(image: Any, factor: float) -> Any:
    return ImageEnhance.Brightness(image).enhance(factor)


def enhance_contrast(image: Any, factor: float) -> Any:
    return ImageEnhance.Contrast(image).enhance(factor)


def enhance_color(image: Any, factor: float) -> Any:
    return ImageEnhance.Color(image).enhance(factor)


def enhance_sharpness(image: Any, factor: float) -> Any:
    return ImageEnhance.Sharpness(image).enhance(factor)


def enhance_brightness_create(image: Any) -> Any:
    return ImageEnhance.Brightness(image)


def enhance_contrast_create(image: Any) -> Any:
    return ImageEnhance.Contrast(image)


def enhance_color_create(image: Any) -> Any:
    return ImageEnhance.Color(image)


def enhance_sharpness_create(image: Any) -> Any:
    return ImageEnhance.Sharpness(image)


def enhance_apply(enhancer: Any, factor: float) -> Any:
    return enhancer.enhance(factor)


# --- ImageChops ---


def chops_difference(image1: Any, image2: Any) -> Any:
    return ImageChops.difference(image1, image2)


def chops_multiply(image1: Any, image2: Any) -> Any:
    return ImageChops.multiply(image1, image2)


def chops_add(image1: Any, image2: Any, scale: float = 1.0, offset: int = 0) -> Any:
    return ImageChops.add(image1, image2, scale, offset)


def chops_subtract(image1: Any, image2: Any, scale: float = 1.0, offset: int = 0) -> Any:
    return ImageChops.subtract(image1, image2, scale, offset)


def chops_blend(image1: Any, image2: Any, alpha: float) -> Any:
    return ImageChops.blend(image1, image2, alpha)


def chops_composite(image1: Any, image2: Any, mask: Any) -> Any:
    return ImageChops.composite(image1, image2, mask)


def chops_lighter(image1: Any, image2: Any) -> Any:
    return ImageChops.lighter(image1, image2)


def chops_darker(image1: Any, image2: Any) -> Any:
    return ImageChops.darker(image1, image2)


def chops_screen(image1: Any, image2: Any) -> Any:
    return ImageChops.screen(image1, image2)


def chops_overlay(image1: Any, image2: Any) -> Any:
    return ImageChops.overlay(image1, image2)


# --- ImageDraw ---


def draw_create(image: Any) -> Any:
    return ImageDraw.Draw(image)


def draw_rectangle(
    draw: Any,
    xy: tuple[int, int, int, int],
    fill: Any = None,
    outline: Any = None,
    width: int = 1,
) -> None:
    draw.rectangle(xy, fill=fill, outline=outline, width=width)


def draw_ellipse(
    draw: Any,
    xy: tuple[int, int, int, int],
    fill: Any = None,
    outline: Any = None,
    width: int = 1,
) -> None:
    draw.ellipse(xy, fill=fill, outline=outline, width=width)


def draw_line(
    draw: Any,
    xy: tuple[int, int, int, int],
    fill: Any = 255,
    width: int = 1,
) -> None:
    draw.line(xy, fill=fill, width=width)


def draw_text(
    draw: Any,
    xy: tuple[int, int],
    text: str,
    fill: Any = 0,
    font_path: str | None = None,
    font_size: int = 20,
) -> None:
    from PIL import ImageFont

    if font_path:
        font = ImageFont.truetype(font_path, font_size)
    else:
        font = ImageFont.load_default(font_size)
    draw.text(xy, text, fill=fill, font=font)


# --- ImageStat ---


def stat_mean(image: Any) -> Any:
    return tuple(ImageStat.Stat(image).mean)


def stat_extrema(image: Any) -> Any:
    return ImageStat.Stat(image).extrema


def stat_count(image: Any) -> Any:
    return tuple(ImageStat.Stat(image).count)


# --- ImageColor ---


def color_getrgb(color: str) -> Any:
    return ImageColor.getrgb(color)


def color_getcolor(color: str, mode: str) -> Any:
    return ImageColor.getcolor(color, mode)


# --- ImageFont ---


def font_truetype(path: str, size: float = 10) -> Any:
    return ImageFont.truetype(path, size)


def font_load_default(size: float | None = None) -> Any:
    return ImageFont.load_default(size)


def font_getbbox(font: Any, text: str) -> tuple[int, int, int, int]:
    return font.getbbox(text)


def font_getlength(font: Any, text: str) -> float:
    return font.getlength(text)


def font_text_bbox(text: str, font_path: str | None, font_size: int) -> tuple[int, int, int, int]:
    font = (
        ImageFont.truetype(font_path, font_size)
        if font_path
        else ImageFont.load_default(font_size)
    )
    return font.getbbox(text)


# --- EXIF ---


def exif_from_image(image: Any) -> Any:
    return image.getexif()


def exif_get_item(exif: Any, tag: int) -> Any:
    return exif.get(tag)


def exif_set_item(exif: Any, tag: int, value: Any) -> None:
    exif[tag] = value


def exif_has_tag(exif: Any, tag: int) -> bool:
    return tag in exif


def exif_list_tags(exif: Any) -> list[int]:
    return list(exif.keys())


def exif_to_bytes(exif: Any) -> bytes:
    return exif.tobytes()


def exif_get_ifd(exif: Any, group: int) -> Any:
    return exif.get_ifd(group)


def exif_ifd_items(exif: Any, group: int) -> list[tuple[int, Any]]:
    return list(exif.get_ifd(group).items())


# --- ImageSequence ---


def sequence_all_frames(image: Any) -> list[Any]:
    return ImageSequence.all_frames(image)


def sequence_frame_count(image: Any) -> int:
    count = 0
    try:
        while True:
            image.seek(count)
            count += 1
    except EOFError:
        pass
    finally:
        if count > 0:
            image.seek(0)
    return count


# --- ImageTransform ---


def transform_affine(
    image: Any,
    size: tuple[int, int],
    matrix: tuple[float, float, float, float, float, float],
    resample: int = 3,
) -> Any:
    return image.transform(
        size,
        ImageTransform.AffineTransform(matrix),
        resample=resample,
    )


def transform_perspective(
    image: Any,
    size: tuple[int, int],
    matrix: tuple[
        float, float, float, float, float, float, float, float
    ],
    resample: int = 3,
) -> Any:
    return image.transform(
        size,
        ImageTransform.PerspectiveTransform(matrix),
        resample=resample,
    )


def transform_extent(
    image: Any,
    size: tuple[int, int],
    bbox: tuple[int, int, int, int],
    resample: int = 3,
) -> Any:
    return image.transform(
        size,
        ImageTransform.ExtentTransform(bbox),
        resample=resample,
    )


def transform_quad(
    image: Any,
    size: tuple[int, int],
    quad: tuple[int, int, int, int, int, int, int, int],
    resample: int = 3,
) -> Any:
    return image.transform(
        size,
        ImageTransform.QuadTransform(quad),
        resample=resample,
    )


# --- ImageMath ---


def imagemath_eval(
    expression: str,
    a: Any | None = None,
    b: Any | None = None,
) -> Any:
    kwargs: dict[str, Any] = {}
    if a is not None:
        kwargs["a"] = a
        kwargs["image"] = a
    if b is not None:
        kwargs["b"] = b
    return ImageMath.unsafe_eval(expression, **kwargs)


# --- Pixel access ---


def pixel_get(image: Any, x: int, y: int) -> Any:
    return image.getpixel((x, y))


def pixel_set(image: Any, x: int, y: int, value: Any) -> None:
    image.putpixel((x, y), value)


def pixel_load(image: Any) -> None:
    image.load()


# --- ImageCms ---


def cms_profile_to_profile(
    image: Any,
    input_profile: str,
    output_profile: str,
    rendering_intent: int = 0,
    output_mode: str | None = None,
) -> Any:
    return ImageCms.profileToProfile(
        image,
        input_profile,
        output_profile,
        rendering_intent,
        outputMode=output_mode,
    )


def cms_get_profile_description(profile_path: str) -> str:
    profile = ImageCms.getOpenProfile(profile_path)
    return ImageCms.getProfileDescription(profile)


def cms_get_open_profile(profile_path: str) -> Any:
    return ImageCms.getOpenProfile(profile_path)


# --- ImageDraw (font object) ---


def draw_text_with_font(
    draw: Any,
    xy: tuple[int, int],
    text: str,
    font: Any,
    fill: Any = 0,
) -> None:
    draw.text(xy, text, fill=fill, font=font)


# --- Watermark ---


def _watermark_clamp_opacity(opacity: float) -> float:
    return max(0.0, min(1.0, opacity))


def watermark_add_text(
    image: Any,
    text: str,
    x: int,
    y: int,
    opacity: float = 0.5,
    fill_r: int = 255,
    fill_g: int = 255,
    fill_b: int = 255,
    font_path: str | None = None,
    font_size: int = 48,
    angle: float = 0,
) -> Any:
    from PIL import ImageDraw, ImageFont

    base = image.convert("RGBA")
    layer = Image.new("RGBA", base.size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(layer)
    font = (
        ImageFont.truetype(font_path, font_size)
        if font_path
        else ImageFont.load_default(font_size)
    )
    alpha = int(_watermark_clamp_opacity(opacity) * 255)
    fill = (fill_r, fill_g, fill_b, alpha)
    bbox = draw.textbbox((x, y), text, font=font)
    draw.text((x, y), text, fill=fill, font=font)
    if angle != 0:
        center = ((bbox[0] + bbox[2]) / 2, (bbox[1] + bbox[3]) / 2)
        layer = layer.rotate(
            angle,
            resample=Image.Resampling.BICUBIC,
            center=center,
            expand=False,
        )
    result = Image.alpha_composite(base, layer)
    return result.convert(image.mode) if image.mode != "RGBA" else result


def watermark_add_image(
    image: Any,
    mark: Any,
    x: int,
    y: int,
    opacity: float = 0.5,
    scale: float = 1.0,
) -> Any:
    base = image.convert("RGBA")
    wm = mark.convert("RGBA")
    if scale != 1.0:
        new_size = (max(1, int(wm.width * scale)), max(1, int(wm.height * scale)))
        wm = wm.resize(new_size, resample=Image.Resampling.LANCZOS)
    opacity = _watermark_clamp_opacity(opacity)
    if opacity < 1.0:
        red, green, blue, alpha = wm.split()
        alpha = alpha.point(lambda value: int(value * opacity))
        wm = Image.merge("RGBA", (red, green, blue, alpha))
    layer = Image.new("RGBA", base.size, (0, 0, 0, 0))
    layer.paste(wm, (x, y), wm)
    result = Image.alpha_composite(base, layer)
    return result.convert(image.mode) if image.mode != "RGBA" else result


def watermark_remove_region(
    image: Any,
    left: int,
    top: int,
    right: int,
    bottom: int,
    method: str = "blur",
    blur_radius: float = 12.0,
) -> Any:
    if left >= right or top >= bottom:
        msg = "Invalid watermark region: left < right and top < bottom are required"
        raise ValueError(msg)

    result = image.copy()
    box = (left, top, right, bottom)
    region = result.crop(box)
    if region.width == 0 or region.height == 0:
        msg = "Watermark region must be non-empty"
        raise ValueError(msg)

    if method == "blur":
        region = region.filter(ImageFilter.GaussianBlur(blur_radius))
    elif method == "median":
        size = max(3, min(region.width, region.height) // 4)
        if size % 2 == 0:
            size += 1
        region = region.filter(ImageFilter.MedianFilter(size))
    elif method == "fill":
        stat = ImageStat.Stat(region)
        mean = stat.mean
        fill: int | tuple[int, ...] = (
            int(mean[0]) if len(mean) == 1 else tuple(int(v) for v in mean)
        )
        region = Image.new(region.mode, region.size, fill)
    else:
        msg = f"Unknown watermark removal method: {method}"
        raise ValueError(msg)
    result.paste(region, box)
    return result
