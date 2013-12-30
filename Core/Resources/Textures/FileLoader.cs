#if DIRECTX11_1

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.WIC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Resources
{
    public static class FileLoader
    {
        [Flags]
        internal enum WICFlags
        {
            None = 0x0,

            ForceRgb = 0x1,
            // Loads DXGI 1.1 BGR formats as DXGI_FORMAT_R8G8B8A8_UNORM to avoid use of optional WDDM 1.1 formats

            NoX2Bias = 0x2,
            // Loads DXGI 1.1 X2 10:10:10:2 format as DXGI_FORMAT_R10G10B10A2_UNORM

            No16Bpp = 0x4,
            // Loads 565, 5551, and 4444 formats as 8888 to avoid use of optional WDDM 1.2 formats

            FlagsAllowMono = 0x8,
            // Loads 1-bit monochrome (black & white) as R1_UNORM rather than 8-bit greyscale

            AllFrames = 0x10,
            // Loads all images in a multi-frame file, converting/resizing to match the first frame as needed, defaults to 0th frame otherwise

            Dither = 0x10000,
            // Use ordered 4x4 dithering for any required conversions

            DitherDiffusion = 0x20000,
            // Use error-diffusion dithering for any required conversions

            FilterPoint = 0x100000,
            FilterLinear = 0x200000,
            FilterCubic = 0x300000,
            FilterFant = 0x400000, // Combination of Linear and Box filter
            // Filtering mode to use for any required image resizing (only needed when loading arrays of differently sized images; defaults to Fant)
        };

        private struct WICTranslate
        {
            public WICTranslate(Guid wic, SharpDX.DXGI.Format format)
            {
                this.WIC = wic;
                this.Format = format;
            }

            public readonly Guid WIC;
            public readonly SharpDX.DXGI.Format Format;
        };

        private static readonly WICTranslate[] WICToDXGIFormats =
            {
                new WICTranslate(SharpDX.WIC.PixelFormat.Format128bppRGBAFloat, SharpDX.DXGI.Format.R32G32B32A32_Float),

                new WICTranslate(SharpDX.WIC.PixelFormat.Format64bppRGBAHalf, SharpDX.DXGI.Format.R16G16B16A16_Float),
                new WICTranslate(SharpDX.WIC.PixelFormat.Format64bppRGBA, SharpDX.DXGI.Format.R16G16B16A16_UNorm),

                new WICTranslate(SharpDX.WIC.PixelFormat.Format32bppRGBA, SharpDX.DXGI.Format.R8G8B8A8_UNorm),
                new WICTranslate(SharpDX.WIC.PixelFormat.Format32bppBGRA, SharpDX.DXGI.Format.B8G8R8A8_UNorm), // DXGI 1.1
                new WICTranslate(SharpDX.WIC.PixelFormat.Format32bppBGR, SharpDX.DXGI.Format.B8G8R8X8_UNorm), // DXGI 1.1

                new WICTranslate(SharpDX.WIC.PixelFormat.Format32bppRGBA1010102XR, SharpDX.DXGI.Format.R10G10B10_Xr_Bias_A2_UNorm), // DXGI 1.1
                new WICTranslate(SharpDX.WIC.PixelFormat.Format32bppRGBA1010102, SharpDX.DXGI.Format.R10G10B10A2_UNorm),
                new WICTranslate(SharpDX.WIC.PixelFormat.Format32bppRGBE, SharpDX.DXGI.Format.R9G9B9E5_Sharedexp),

                new WICTranslate(SharpDX.WIC.PixelFormat.Format16bppBGRA5551, SharpDX.DXGI.Format.B5G5R5A1_UNorm),
                new WICTranslate(SharpDX.WIC.PixelFormat.Format16bppBGR565, SharpDX.DXGI.Format.B5G6R5_UNorm),

                new WICTranslate(SharpDX.WIC.PixelFormat.Format32bppGrayFloat, SharpDX.DXGI.Format.R32_Float),
                new WICTranslate(SharpDX.WIC.PixelFormat.Format16bppGrayHalf, SharpDX.DXGI.Format.R16_Float),
                new WICTranslate(SharpDX.WIC.PixelFormat.Format16bppGray, SharpDX.DXGI.Format.R16_UNorm),
                new WICTranslate(SharpDX.WIC.PixelFormat.Format8bppGray, SharpDX.DXGI.Format.R8_UNorm),

                new WICTranslate(SharpDX.WIC.PixelFormat.Format8bppAlpha, SharpDX.DXGI.Format.A8_UNorm),

                new WICTranslate(SharpDX.WIC.PixelFormat.FormatBlackWhite, SharpDX.DXGI.Format.R1_UNorm),

#if DIRECTX11_1
                new WICTranslate(SharpDX.WIC.PixelFormat.Format96bppRGBFloat,         SharpDX.DXGI.Format.R32G32B32_Float ),
        #endif
            };
    

        public static SharpDX.WIC.BitmapSource LoadBitmap(SharpDX.WIC.ImagingFactory2 factory, string filename)
        {
            var bitmapDecoder = new SharpDX.WIC.BitmapDecoder(
                factory,
                filename,
                SharpDX.WIC.DecodeOptions.CacheOnDemand
                );

            var formatConverter = new SharpDX.WIC.FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                SharpDX.WIC.PixelFormat.Format32bppPRGBA,
                SharpDX.WIC.BitmapDitherType.None,
                null,
                0.0,
                SharpDX.WIC.BitmapPaletteType.Custom);

            return formatConverter;
        }

        public static SharpDX.Direct3D11.Texture2D CreateTexture2DFromBitmap(SharpDX.Direct3D11.Device device, SharpDX.WIC.BitmapSource bitmapSource, bool mips = true)
        {
            // Allocate DataStream to receive the WIC image pixels
            int stride = bitmapSource.Size.Width * 4;
            using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                bitmapSource.CopyPixels(stride, buffer);
                Texture2D texture = new SharpDX.Direct3D11.Texture2D(device, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = mips ? BindFlags.RenderTarget | BindFlags.ShaderResource : BindFlags.ShaderResource,
                    Usage = mips ? ResourceUsage.Default : ResourceUsage.Immutable,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                    MipLevels = mips ? 0 : 1,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                }, new SharpDX.DataRectangle(buffer.DataPointer, stride));

                return texture;
            }
        }

        private static BitmapDitherType GetWICDither(WICFlags flags)
        {
            if ((flags & WICFlags.Dither) != 0)
                return BitmapDitherType.Ordered4x4;

            if ((flags & WICFlags.DitherDiffusion) != 0)
                return BitmapDitherType.ErrorDiffusion;

            return BitmapDitherType.None;
        }

        private static int GetBitsPerPixel(SharpDX.WIC.ImagingFactory2 factory, Guid targetGuid)
        {
            using (var info = new ComponentInfo(factory, targetGuid))
            {
                if (info.ComponentType != ComponentType.PixelFormat)
                    return 0;

                var pixelFormatInfo = info.QueryInterfaceOrNull<PixelFormatInfo>();
                if (pixelFormatInfo == null)
                    return 0;

                int bpp = pixelFormatInfo.BitsPerPixel;
                pixelFormatInfo.Dispose();
                return bpp;
            }
        }

        public static void SaveToFile(SharpDX.WIC.ImagingFactory2 factory,RenderContext context, ImageFileFormat fileFormat, DX11Texture2D staging, string path)
        {
            DataBox ds = staging.MapForRead(context);
            Guid uid = GetContainerFormatFromFileType(fileFormat);
            WICFlags flags = WICFlags.None;

            FileStream fs = new FileStream(path, FileMode.Create);

            using (var encoder = new BitmapEncoder(factory, uid,fs))
            {
                using (var frame = new BitmapFrameEncode(encoder))
                {
                    if (uid == ContainerFormatGuids.Bmp)
                    {
                        try
                        {
                            frame.Options.Set("EnableV5Header32bppBGRA", true);
                        }
                        catch
                        {
                        }
                    }

                    Guid pfGuid;
                    if (!ToWIC(staging.Format, out pfGuid))
                        throw new NotSupportedException("Format not supported");



                    frame.Initialize();
                    frame.SetSize(staging.Width, staging.Height);
                    frame.SetResolution(72, 72);
                    Guid targetGuid = pfGuid;
                    frame.SetPixelFormat(ref targetGuid);


                    if (targetGuid != pfGuid)
                    {
                        using (var source = new Bitmap(factory, staging.Width, staging.Height, pfGuid, new DataRectangle(ds.DataPointer, ds.RowPitch), 0))
                        {
                            using (var converter = new FormatConverter(factory))
                            {
                                using (var palette = new Palette(factory))
                                {
                                    palette.Initialize(source, 256, true);
                                    converter.Initialize(source, targetGuid, GetWICDither(flags), palette, 0, BitmapPaletteType.Custom);

                                    int bpp = GetBitsPerPixel(factory,targetGuid);
                                    if (bpp == 0) throw new NotSupportedException("Unable to determine the Bpp for the target format");

                                    int rowPitch = (staging.Width * bpp + 7) / 8;
                                    int slicePitch = rowPitch * staging.Height;

                                    var temp = Utilities.AllocateMemory(slicePitch);
                                    try
                                    {
                                        converter.CopyPixels(rowPitch, temp, slicePitch);
                                        frame.Palette = palette;
                                        frame.WritePixels(staging.Height, temp, rowPitch, slicePitch);
                                    }
                                    finally
                                    {
                                        Utilities.FreeMemory(temp);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        frame.WritePixels(staging.Height, ds.DataPointer, ds.RowPitch, 0);
                    }
                    frame.Commit();

                    encoder.Commit();
                }
            }
            staging.UnMap(context);

            fs.Dispose();
        }

        private static bool ToWIC(SharpDX.DXGI.Format format, out Guid guid)
        {
            for (int i = 0; i < WICToDXGIFormats.Length; ++i)
            {
                if (WICToDXGIFormats[i].Format == format)
                {
                    guid = WICToDXGIFormats[i].WIC;
                    return true;
                }
            }

            // Special cases
            switch (format)
            {
                case SharpDX.DXGI.Format.R8G8B8A8_UNorm_SRgb:
                    guid = SharpDX.WIC.PixelFormat.Format32bppRGBA;
                    return true;

                case SharpDX.DXGI.Format.D32_Float:
                    guid = SharpDX.WIC.PixelFormat.Format32bppGrayFloat;
                    return true;

                case SharpDX.DXGI.Format.D16_UNorm:
                    guid = SharpDX.WIC.PixelFormat.Format16bppGray;
                    return true;

                case SharpDX.DXGI.Format.B8G8R8A8_UNorm_SRgb:
                    guid = SharpDX.WIC.PixelFormat.Format32bppBGRA;
                    return true;

                case SharpDX.DXGI.Format.B8G8R8X8_UNorm_SRgb:
                    guid = SharpDX.WIC.PixelFormat.Format32bppBGR;
                    return true;
            }

            guid = Guid.Empty;
            return false;
        }

        private static Guid GetContainerFormatFromFileType(ImageFileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case ImageFileFormat.Bmp:
                    return ContainerFormatGuids.Bmp;
                case ImageFileFormat.Jpg:
                    return ContainerFormatGuids.Jpeg;
                case ImageFileFormat.Gif:
                    return ContainerFormatGuids.Gif;
                case ImageFileFormat.Png:
                    return ContainerFormatGuids.Png;
                case ImageFileFormat.Tiff:
                    return ContainerFormatGuids.Tiff;
                case ImageFileFormat.Wmp:
                    return ContainerFormatGuids.Wmp;
                default:
                    throw new NotSupportedException("Format not supported");
            }
        }

    }
}

#endif