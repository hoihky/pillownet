using System.CommandLine;
using PillowNet;

return await RunAsync(args);

static async Task<int> RunAsync(string[] args)
{
    var root = new RootCommand("PillowNet — image processing powered by Pillow via CSnakes");

    root.Add(ConvertCommand());
    root.Add(ResizeCommand());
    root.Add(ThumbnailCommand());
    root.Add(FilterCommand());
    root.Add(EnhanceCommand());
    root.Add(AdjustCommand());
    root.Add(FlipCommand());
    root.Add(CompositeCommand());
    root.Add(DrawCommand());
    root.Add(ExifCommand());
    root.Add(FramesCommand());
    root.Add(MathCommand());
    root.Add(PixelCommand());
    root.Add(WatermarkAddTextCommand());
    root.Add(WatermarkAddImageCommand());
    root.Add(WatermarkRemoveCommand());
    root.Add(InfoCommand());

    return await root.Parse(args).InvokeAsync();
}

static Command ConvertCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var format = new Option<string?>("--format", "-f") { Description = "Output format (e.g. PNG, JPEG, WEBP)" };
    var mode = new Option<string?>("--mode", "-m") { Description = "Color mode to convert to (e.g. RGB, L, RGBA)" };

    var command = new Command("convert", "Convert image format and/or color mode")
    {
        input,
        output,
        format,
        mode,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var outputFormat = parseResult.GetValue(format);
        var colorMode = parseResult.GetValue(mode);

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        if (colorMode is not null)
        {
            using var converted = image.Convert(colorMode);
            converted.Save(outputFile.FullName, outputFormat);
        }
        else
        {
            image.Save(outputFile.FullName, outputFormat);
        }

        Console.WriteLine($"Saved {outputFile.FullName}");
    });

    return command;
}

static Command ResizeCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var width = new Option<int>("--width", "-w") { Description = "Target width in pixels", Required = true };
    var height = new Option<int>("--height", "-h") { Description = "Target height in pixels", Required = true };
    var resample = new Option<Resampling>("--resample")
    {
        Description = "Resampling filter (Nearest, Lanczos, Bilinear, Bicubic, Box, Hamming)",
        DefaultValueFactory = _ => Resampling.Lanczos,
    };

    var command = new Command("resize", "Resize an image to exact dimensions")
    {
        input,
        output,
        width,
        height,
        resample,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var w = parseResult.GetValue(width);
        var h = parseResult.GetValue(height);
        var filter = parseResult.GetValue(resample);

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        using var resized = image.Resize((w, h), filter);
        resized.Save(outputFile.FullName);
        Console.WriteLine($"Resized to {w}x{h} -> {outputFile.FullName}");
    });

    return command;
}

static Command ThumbnailCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var maxSize = new Option<int>("--max-size")
    {
        Description = "Maximum width and height",
        DefaultValueFactory = _ => 128,
    };

    var command = new Command("thumbnail", "Create a thumbnail preserving aspect ratio")
    {
        input,
        output,
        maxSize,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var size = parseResult.GetValue(maxSize);

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        image.Thumbnail((size, size));
        image.Save(outputFile.FullName);
        Console.WriteLine($"Thumbnail max {size}px -> {outputFile.FullName}");
    });

    return command;
}

static Command FilterCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var filterName = new Option<string>("--filter", "-f")
    {
        Description = "Filter name (Blur, Sharpen, Contour, Detail, EdgeEnhance, Emboss, FindEdges, Smooth, GaussianBlur, BoxBlur, UnsharpMask)",
        Required = true,
    };
    var radius = new Option<double>("--radius")
    {
        Description = "Radius for GaussianBlur, BoxBlur, or UnsharpMask",
        DefaultValueFactory = _ => 2.0,
    };

    var command = new Command("filter", "Apply an image filter")
    {
        input,
        output,
        filterName,
        radius,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var name = parseResult.GetValue(filterName)!;
        var r = parseResult.GetValue(radius);

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        var filter = ResolveFilter(name, r);
        using var filtered = image.Filter(filter);
        filtered.Save(outputFile.FullName);
        Console.WriteLine($"Applied {name} -> {outputFile.FullName}");
    });

    return command;
}

static Command EnhanceCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var operation = new Option<string>("--op", "-o")
    {
        Description = "Operation (Autocontrast, Equalize, Grayscale, Invert, Posterize, Solarize, Flip, Mirror, ExifTranspose)",
        Required = true,
    };
    var bits = new Option<int>("--bits")
    {
        Description = "Bits for Posterize (1-8)",
        DefaultValueFactory = _ => 4,
    };
    var threshold = new Option<int>("--threshold")
    {
        Description = "Threshold for Solarize (0-255)",
        DefaultValueFactory = _ => 128,
    };

    var command = new Command("enhance", "Apply ImageOps enhancements")
    {
        input,
        output,
        operation,
        bits,
        threshold,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var op = parseResult.GetValue(operation)!;
        var posterizeBits = parseResult.GetValue(bits);
        var solarizeThreshold = parseResult.GetValue(threshold);

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        using var result = op.ToLowerInvariant() switch
        {
            "autocontrast" => ImageOps.Autocontrast(image),
            "equalize" => ImageOps.Equalize(image),
            "grayscale" => ImageOps.Grayscale(image),
            "invert" => ImageOps.Invert(image),
            "posterize" => ImageOps.Posterize(image, posterizeBits),
            "solarize" => ImageOps.Solarize(image, solarizeThreshold),
            "flip" => ImageOps.Flip(image),
            "mirror" => ImageOps.Mirror(image),
            "exiftranspose" => ImageOps.ExifTranspose(image),
            _ => throw new ArgumentException($"Unknown operation: {op}"),
        };

        result.Save(outputFile.FullName);
        Console.WriteLine($"Applied {op} -> {outputFile.FullName}");
    });

    return command;
}

static Command AdjustCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var brightness = new Option<double?>("--brightness") { Description = "Brightness factor (1.0 = original)" };
    var contrast = new Option<double?>("--contrast") { Description = "Contrast factor (1.0 = original)" };
    var sharpness = new Option<double?>("--sharpness") { Description = "Sharpness factor (1.0 = original)" };
    var color = new Option<double?>("--color") { Description = "Color saturation factor (1.0 = original)" };

    var command = new Command("adjust", "Adjust brightness, contrast, sharpness, or color")
    {
        input,
        output,
        brightness,
        contrast,
        sharpness,
        color,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var b = parseResult.GetValue(brightness);
        var c = parseResult.GetValue(contrast);
        var s = parseResult.GetValue(sharpness);
        var col = parseResult.GetValue(color);

        if (b is null && c is null && s is null && col is null)
        {
            throw new ArgumentException("Specify at least one of --brightness, --contrast, --sharpness, --color");
        }

        PillowEnvironment.Initialize();
        Image current = Image.Open(inputFile.FullName);
        try
        {
            if (b is not null)
            {
                var next = ImageEnhance.Brightness(current, b.Value);
                if (!ReferenceEquals(current, next)) current.Dispose();
                current = next;
            }

            if (c is not null)
            {
                var next = ImageEnhance.Contrast(current, c.Value);
                if (!ReferenceEquals(current, next)) current.Dispose();
                current = next;
            }

            if (s is not null)
            {
                var next = ImageEnhance.Sharpness(current, s.Value);
                if (!ReferenceEquals(current, next)) current.Dispose();
                current = next;
            }

            if (col is not null)
            {
                var next = ImageEnhance.Color(current, col.Value);
                if (!ReferenceEquals(current, next)) current.Dispose();
                current = next;
            }

            current.Save(outputFile.FullName);
            Console.WriteLine($"Adjusted -> {outputFile.FullName}");
        }
        finally
        {
            current.Dispose();
        }
    });

    return command;
}

static Command FlipCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var horizontal = new Option<bool>("--horizontal") { Description = "Flip horizontally (default: vertical)" };

    var command = new Command("flip", "Flip image vertically or horizontally")
    {
        input,
        output,
        horizontal,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var isHorizontal = parseResult.GetValue(horizontal);

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        using var flipped = isHorizontal ? ImageOps.Mirror(image) : ImageOps.Flip(image);
        flipped.Save(outputFile.FullName);
        Console.WriteLine($"Flipped -> {outputFile.FullName}");
    });

    return command;
}

static Command CompositeCommand()
{
    var baseImage = new Argument<FileInfo>("base") { Description = "Base image path" };
    var overlay = new Argument<FileInfo>("overlay") { Description = "Overlay image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var alpha = new Option<double>("--alpha")
    {
        Description = "Blend alpha (0.0 = base only, 1.0 = overlay only)",
        DefaultValueFactory = _ => 0.5,
    };

    var command = new Command("composite", "Blend two images together")
    {
        baseImage,
        overlay,
        output,
        alpha,
    };

    command.SetAction(parseResult =>
    {
        var baseFile = parseResult.GetValue(baseImage)!;
        var overlayFile = parseResult.GetValue(overlay)!;
        var outputFile = parseResult.GetValue(output)!;
        var blendAlpha = parseResult.GetValue(alpha);

        PillowEnvironment.Initialize();
        using var a = Image.Open(baseFile.FullName);
        using var b = Image.Open(overlayFile.FullName);
        using var result = ImageChops.Blend(a, b, blendAlpha);
        result.Save(outputFile.FullName);
        Console.WriteLine($"Composited (alpha={blendAlpha}) -> {outputFile.FullName}");
    });

    return command;
}

static Command DrawCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var text = new Option<string>("--text", "-t") { Description = "Text to draw", Required = true };
    var x = new Option<int>("--x") { Description = "X position", DefaultValueFactory = _ => 10 };
    var y = new Option<int>("--y") { Description = "Y position", DefaultValueFactory = _ => 10 };
    var font = new Option<string?>("--font") { Description = "Path to TrueType font file" };
    var fontSize = new Option<int>("--font-size") { Description = "Font size", DefaultValueFactory = _ => 24 };

    var command = new Command("draw", "Draw text on an image")
    {
        input,
        output,
        text,
        x,
        y,
        font,
        fontSize,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var label = parseResult.GetValue(text)!;
        var px = parseResult.GetValue(x);
        var py = parseResult.GetValue(y);
        var fontPath = parseResult.GetValue(font);
        var size = parseResult.GetValue(fontSize);

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        using (var draw = new ImageDraw(image))
        {
            draw.Text((px, py), label, fill: (255, 255, 255), fontPath: fontPath, fontSize: size);
        }

        image.Save(outputFile.FullName);
        Console.WriteLine($"Drew text -> {outputFile.FullName}");
    });

    return command;
}

static Command ExifCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };

    var command = new Command("exif", "Print EXIF metadata tags")
    {
        input,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        using var exif = image.GetExif();
        var tags = exif.Tags;
        if (tags.Count == 0)
        {
            Console.WriteLine("No EXIF tags found.");
            return;
        }

        foreach (var tag in tags.OrderBy(t => t))
        {
            using var value = exif.Get(tag);
            Console.WriteLine($"0x{tag:X4} ({tag}): {value}");
        }
    });

    return command;
}

static Command FramesCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Animated image path" };
    var outputDir = new Argument<DirectoryInfo>("output-dir") { Description = "Directory for frame files" };
    var prefix = new Option<string>("--prefix")
    {
        Description = "Output filename prefix",
        DefaultValueFactory = _ => "frame",
    };

    var command = new Command("frames", "Extract animation frames")
    {
        input,
        outputDir,
        prefix,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var dir = parseResult.GetValue(outputDir)!;
        var name = parseResult.GetValue(prefix)!;

        Directory.CreateDirectory(dir.FullName);
        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        var count = ImageSequence.FrameCount(image);
        var frames = ImageSequence.AllFrames(image);

        for (var i = 0; i < frames.Count; i++)
        {
            using var frame = frames[i];
            var path = Path.Combine(dir.FullName, $"{name}_{i:D4}.png");
            frame.Save(path);
            Console.WriteLine($"Wrote {path}");
        }

        Console.WriteLine($"Extracted {frames.Count} frame(s) (reported count: {count}).");
    });

    return command;
}

static Command MathCommand()
{
    var a = new Argument<FileInfo>("a") { Description = "First image (bound as a/image)" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var expression = new Option<string>("--expr", "-e")
    {
        Description = "ImageMath expression (e.g. \"a * 2\", \"a + b\")",
        Required = true,
    };
    var b = new Option<FileInfo?>("--b") { Description = "Second image for binary expressions" };

    var command = new Command("math", "Evaluate a Pillow ImageMath expression")
    {
        a,
        output,
        expression,
        b,
    };

    command.SetAction(parseResult =>
    {
        var aFile = parseResult.GetValue(a)!;
        var outputFile = parseResult.GetValue(output)!;
        var expr = parseResult.GetValue(expression)!;
        var bFile = parseResult.GetValue(b);

        PillowEnvironment.Initialize();
        using var imageA = Image.Open(aFile.FullName);
        if (bFile is null)
        {
            using var result = ImageMath.Eval(expr, imageA);
            result.Save(outputFile.FullName);
        }
        else
        {
            using var imageB = Image.Open(bFile.FullName);
            using var result = ImageMath.Eval(expr, imageA, imageB);
            result.Save(outputFile.FullName);
        }

        Console.WriteLine($"Evaluated \"{expr}\" -> {outputFile.FullName}");
    });

    return command;
}

static Command PixelCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var x = new Option<int>("--x") { Description = "X coordinate", Required = true };
    var y = new Option<int>("--y") { Description = "Y coordinate", Required = true };
    var value = new Option<long?>("--set") { Description = "If set, writes this grayscale pixel value" };
    var output = new Option<FileInfo?>("--output", "-o") { Description = "Save image after writing a pixel" };

    var command = new Command("pixel", "Read or write a single pixel")
    {
        input,
        x,
        y,
        value,
        output,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var px = parseResult.GetValue(x);
        var py = parseResult.GetValue(y);
        var setValue = parseResult.GetValue(value);
        var outputFile = parseResult.GetValue(output);

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        if (setValue is null)
        {
            var pixel = image.GetPixel(px, py);
            Console.WriteLine($"({px}, {py}) = {pixel}");
            return;
        }

        image.PutPixel(px, py, setValue.Value);
        Console.WriteLine($"Set ({px}, {py}) = {setValue.Value}");
        var savePath = outputFile?.FullName ?? inputFile.FullName;
        image.Save(savePath);
        Console.WriteLine($"Saved {savePath}");
    });

    return command;
}

static Command WatermarkAddTextCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var text = new Option<string>("--text", "-t") { Description = "Watermark text", Required = true };
    var opacity = new Option<double>("--opacity")
    {
        Description = "Opacity from 0.0 to 1.0",
        DefaultValueFactory = _ => 0.5,
    };
    var x = new Option<int?>("--x") { Description = "X position (overrides anchor)" };
    var y = new Option<int?>("--y") { Description = "Y position (overrides anchor)" };
    var anchor = new Option<WatermarkAnchor>("--anchor")
    {
        Description = "Position when --x/--y are omitted",
        DefaultValueFactory = _ => WatermarkAnchor.BottomRight,
    };
    var margin = new Option<int>("--margin")
    {
        Description = "Margin from edge when using anchor",
        DefaultValueFactory = _ => 16,
    };
    var fontSize = new Option<int>("--font-size")
    {
        Description = "Font size in pixels",
        DefaultValueFactory = _ => 48,
    };
    var font = new Option<string?>("--font") { Description = "Path to TrueType font file" };
    var angle = new Option<double>("--angle")
    {
        Description = "Rotation angle in degrees",
        DefaultValueFactory = _ => 0.0,
    };
    var color = new Option<string>("--color")
    {
        Description = "Text color name or #RRGGBB",
        DefaultValueFactory = _ => "white",
    };

    var command = new Command("watermark-text", "Add a semi-transparent text watermark")
    {
        input,
        output,
        text,
        opacity,
        x,
        y,
        anchor,
        margin,
        fontSize,
        font,
        angle,
        color,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;
        var label = parseResult.GetValue(text)!;
        var px = parseResult.GetValue(x);
        var py = parseResult.GetValue(y);
        if (px is null ^ py is null)
        {
            throw new InvalidOperationException("Specify both --x and --y, or omit both to use --anchor.");
        }

        PillowEnvironment.Initialize();
        var rgb = ImageColor.GetRgb(parseResult.GetValue(color)!);
        var options = new WatermarkOptions
        {
            Position = px is not null && py is not null ? (px.Value, py.Value) : null,
            Anchor = parseResult.GetValue(anchor),
            Margin = parseResult.GetValue(margin),
            Opacity = parseResult.GetValue(opacity),
            FontSize = parseResult.GetValue(fontSize),
            FontPath = parseResult.GetValue(font),
            Angle = parseResult.GetValue(angle),
            Color = (rgb[0], rgb[1], rgb[2]),
        };

        using var image = Image.Open(inputFile.FullName);
        using var result = Watermark.AddText(image, label, options);
        result.Save(outputFile.FullName);
        Console.WriteLine($"Watermarked -> {outputFile.FullName}");
    });

    return command;
}

static Command WatermarkAddImageCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var watermark = new Argument<FileInfo>("watermark") { Description = "Watermark image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var opacity = new Option<double>("--opacity")
    {
        Description = "Opacity from 0.0 to 1.0",
        DefaultValueFactory = _ => 0.5,
    };
    var scale = new Option<double>("--scale")
    {
        Description = "Scale factor for the watermark image",
        DefaultValueFactory = _ => 1.0,
    };
    var x = new Option<int?>("--x") { Description = "X position (overrides anchor)" };
    var y = new Option<int?>("--y") { Description = "Y position (overrides anchor)" };
    var anchor = new Option<WatermarkAnchor>("--anchor")
    {
        Description = "Position when --x/--y are omitted",
        DefaultValueFactory = _ => WatermarkAnchor.BottomRight,
    };
    var margin = new Option<int>("--margin")
    {
        Description = "Margin from edge when using anchor",
        DefaultValueFactory = _ => 16,
    };

    var command = new Command("watermark-image", "Add a semi-transparent image watermark")
    {
        input,
        watermark,
        output,
        opacity,
        scale,
        x,
        y,
        anchor,
        margin,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var markFile = parseResult.GetValue(watermark)!;
        var outputFile = parseResult.GetValue(output)!;
        var px = parseResult.GetValue(x);
        var py = parseResult.GetValue(y);
        if (px is null ^ py is null)
        {
            throw new InvalidOperationException("Specify both --x and --y, or omit both to use --anchor.");
        }

        var options = new WatermarkOptions
        {
            Position = px is not null && py is not null ? (px.Value, py.Value) : null,
            Anchor = parseResult.GetValue(anchor),
            Margin = parseResult.GetValue(margin),
            Opacity = parseResult.GetValue(opacity),
            Scale = parseResult.GetValue(scale),
        };

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        using var mark = Image.Open(markFile.FullName);
        using var result = Watermark.AddImage(image, mark, options);
        result.Save(outputFile.FullName);
        Console.WriteLine($"Watermarked -> {outputFile.FullName}");
    });

    return command;
}

static Command WatermarkRemoveCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };
    var output = new Argument<FileInfo>("output") { Description = "Output image path" };
    var left = new Option<int>("--left", "-l") { Description = "Left edge of watermark region", Required = true };
    var top = new Option<int>("--top", "-t") { Description = "Top edge of watermark region", Required = true };
    var right = new Option<int>("--right", "-r") { Description = "Right edge of watermark region", Required = true };
    var bottom = new Option<int>("--bottom", "-b") { Description = "Bottom edge of watermark region", Required = true };
    var method = new Option<WatermarkRemovalMethod>("--method")
    {
        Description = "Removal method (Blur, Median, Fill)",
        DefaultValueFactory = _ => WatermarkRemovalMethod.Blur,
    };
    var radius = new Option<double>("--radius")
    {
        Description = "Blur radius when method is Blur",
        DefaultValueFactory = _ => 12.0,
    };

    var command = new Command("watermark-remove", "Remove or obscure a watermark in a rectangular region")
    {
        input,
        output,
        left,
        top,
        right,
        bottom,
        method,
        radius,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;
        var outputFile = parseResult.GetValue(output)!;

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        using var result = Watermark.RemoveRegion(
            image,
            (
                parseResult.GetValue(left),
                parseResult.GetValue(top),
                parseResult.GetValue(right),
                parseResult.GetValue(bottom)),
            parseResult.GetValue(method),
            parseResult.GetValue(radius));
        result.Save(outputFile.FullName);
        Console.WriteLine($"Removed watermark region -> {outputFile.FullName}");
    });

    return command;
}

static Command InfoCommand()
{
    var input = new Argument<FileInfo>("input") { Description = "Input image path" };

    var command = new Command("info", "Print image metadata")
    {
        input,
    };

    command.SetAction(parseResult =>
    {
        var inputFile = parseResult.GetValue(input)!;

        PillowEnvironment.Initialize();
        using var image = Image.Open(inputFile.FullName);
        Console.WriteLine($"Path:   {inputFile.FullName}");
        Console.WriteLine($"Size:   {image.Width} x {image.Height}");
        Console.WriteLine($"Mode:   {image.Mode}");
        Console.WriteLine($"Format: {image.Format ?? "(unknown)"}");

        var mean = ImageStat.Mean(image);
        Console.WriteLine($"Mean:   [{string.Join(", ", mean.Select(m => m.ToString("F1")))}]");
    });

    return command;
}

static IImageFilter ResolveFilter(string name, double radius) =>
    name.ToLowerInvariant() switch
    {
        "blur" => ImageFilter.Blur,
        "sharpen" => ImageFilter.Sharpen,
        "contour" => ImageFilter.Contour,
        "detail" => ImageFilter.Detail,
        "edgeenhance" => ImageFilter.EdgeEnhance,
        "edgeenhancemore" => ImageFilter.EdgeEnhanceMore,
        "emboss" => ImageFilter.Emboss,
        "findedges" => ImageFilter.FindEdges,
        "smooth" => ImageFilter.Smooth,
        "smoothmore" => ImageFilter.SmoothMore,
        "gaussianblur" => ImageFilter.GaussianBlur(radius),
        "boxblur" => ImageFilter.BoxBlur(radius),
        "unsharpmask" => ImageFilter.UnsharpMask(radius),
        "min" => ImageFilter.Min(),
        "max" => ImageFilter.Max(),
        "median" => ImageFilter.Median(),
        "mode" => ImageFilter.Mode(),
        _ => throw new ArgumentException($"Unknown filter: {name}"),
    };
