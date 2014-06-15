using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;




namespace FeralTic.DX11.Resources
{

    public enum eImageFormat { Bmp = 1, Jpeg = 2, Png = 3, Tiff = 4, Gif = 5, Hdp = 6,Dds = 128, Tga = 129 }

    public enum DdsBlockType
    {
        BC1 = SharpDX.DXGI.Format.BC1_UNorm,
        BC2 = SharpDX.DXGI.Format.BC2_UNorm,
        BC3 = SharpDX.DXGI.Format.BC3_UNorm,
        BC7 = SharpDX.DXGI.Format.BC7_UNorm
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageMetadata
    {
        public long Width;
        public long Height;
        public long Depth;
        public long ArraySize;
        public long MipLevels;
        public int MiscFlags;
        public int MiscFlags2;
        public SharpDX.DXGI.Format Format;
        public SharpDX.Direct3D11.ResourceDimension Dimension;
    }


    public class TextureLoader
    {
        private class NativeMethods
        {
            public static ImageMetadata LoadMetadataFromFile(string path)
            {
                if (IntPtr.Size == 8)
                {
                    return NativeMethods64.LoadMetadataFromFile(path);
                }
                else
                {
                    return NativeMethods32.LoadMetadataFromFile(path);
                }
            }

            public static ImageMetadata LoadMetadataFromMemory(IntPtr dataPointer, int dataLength)
            {
                if (IntPtr.Size == 8)
                {
                    return NativeMethods64.LoadMetadataFromMemory(dataPointer, dataLength);
                }
                else
                {
                    return NativeMethods32.LoadMetadataFromMemory(dataPointer, dataLength);
                }
            }

            public static long LoadTextureFromFile(IntPtr device, string path,out IntPtr resource, int miplevels)
            {
                if (IntPtr.Size == 8)
                {
                    return NativeMethods64.LoadTextureFromFile(device, path, out resource, miplevels);
                }
                else
                {
                    return NativeMethods32.LoadTextureFromFile(device, path, out resource, miplevels);
                }
            }

            public static long SaveTextureToFile(IntPtr device, IntPtr context, IntPtr resource, string path, int format)
            {
                if (IntPtr.Size == 8)
                {
                    return NativeMethods64.SaveTextureToFile(device, context,resource, path, format);
                }
                else
                {
                    return NativeMethods32.SaveTextureToFile(device, context, resource, path, format);
                }
            }

            public static long SaveCompressedTextureToFile(IntPtr device, IntPtr context, IntPtr resource, string path, int blocktype)
            {
                if (IntPtr.Size == 8)
                {
                    return NativeMethods64.SaveCompressedTextureToFile(device, context, resource, path, blocktype);
                }
                else
                {
                    return NativeMethods32.SaveCompressedTextureToFile(device, context, resource, path, blocktype);
                }
            }

            public static long LoadTextureFromMemory(IntPtr device, IntPtr dataPointer, int dataLength, out IntPtr resource)
            {
                if (IntPtr.Size == 8)
                {
                    return NativeMethods64.LoadTextureFromMemory(device, dataPointer, dataLength, out resource);
                }
                else
                {
                    return NativeMethods32.LoadTextureFromMemory(device, dataPointer, dataLength, out resource);
                }
            }
        }

        private class NativeMethods64
        {
            [DllImport("DirectXTexLib_x64", CharSet = CharSet.Unicode)]
            public static extern ImageMetadata LoadMetadataFromFile(string path);

            [DllImport("DirectXTexLib_x64", CharSet = CharSet.Unicode)]
            public static extern ImageMetadata LoadMetadataFromMemory(IntPtr dataPointer, int dataLength);

            [DllImport("DirectXTexLib_x64", CharSet = CharSet.Unicode)]
            public static extern long LoadTextureFromFile(IntPtr device, string path, out IntPtr resource, int miplevels);

            [DllImport("DirectXTexLib_x64", CharSet = CharSet.Unicode)]
            public static extern long LoadTextureFromMemory(IntPtr device, IntPtr dataPointer, int dataLength, out IntPtr resource);

            [DllImport("DirectXTexLib_x64", CharSet = CharSet.Unicode)]
            public static extern long SaveTextureToFile(IntPtr device, IntPtr context, IntPtr resource, string path, int format);

            [DllImport("DirectXTexLib_x64", CharSet = CharSet.Unicode)]
            public static extern long SaveCompressedTextureToFile(IntPtr device, IntPtr context, IntPtr resource, string path, int blocktype);
        }


        private class NativeMethods32
        {
            [DllImport("DirectXTexLib_x86", CharSet = CharSet.Unicode)]
            public static extern ImageMetadata LoadMetadataFromFile(string path);

            [DllImport("DirectXTexLib_x86", CharSet = CharSet.Unicode)]
            public static extern ImageMetadata LoadMetadataFromMemory(IntPtr dataPointer, int dataLength);

            [DllImport("DirectXTexLib_x86",CharSet=CharSet.Unicode)]
            public static extern long LoadTextureFromFile(IntPtr device, string path,out IntPtr resource, int miplevels);

            [DllImport("DirectXTexLib_x86", CharSet = CharSet.Unicode)]
            public static extern long SaveTextureToFile(IntPtr device,IntPtr context,IntPtr resource, string path,int format);

            [DllImport("DirectXTexLib_x86", CharSet = CharSet.Unicode)]
            public static extern long LoadTextureFromMemory(IntPtr device, IntPtr dataPointer, int dataLength, out IntPtr resource);

            [DllImport("DirectXTexLib_x86", CharSet = CharSet.Unicode)]
            public static extern long SaveCompressedTextureToFile(IntPtr device, IntPtr context, IntPtr resource, string path, int blocktype);
        }

        public static DX11Texture2D LoadFromFile(DxDevice device, string path, bool mips = true)
        {
            IntPtr resource;
            int levels = mips ? 0 : 1;
            NativeMethods.LoadTextureFromFile(device.Device.NativePointer, path, out resource, levels);

            if (resource != IntPtr.Zero)
            {
                Texture2D texture = Texture2D.FromPointer<Texture2D>(resource);
                ShaderResourceView srv = new ShaderResourceView(device,texture);
                return DX11Texture2D.FromReference(device, texture, srv);
            }
            else
            {
                throw new Exception("Failed to load texture");
            }
        }

        public static DX11Texture2D LoadFromMemory(DxDevice device, IntPtr dataPointer, int dataLength)
        {
            IntPtr resource;
            NativeMethods.LoadTextureFromMemory(device.Device.NativePointer, dataPointer, dataLength, out resource);

            if (resource != IntPtr.Zero)
            {
                Texture2D texture = Texture2D.FromPointer<Texture2D>(resource);
                ShaderResourceView srv = new ShaderResourceView(device, texture);
                return DX11Texture2D.FromReference(device, texture, srv);
            }
            else
            {
                throw new Exception("Failed to load texture");
            }
        }

        public static DX11TextureCube LoadCubeTextureFromFile(DxDevice device, string path, bool mips = true)
        {
            IntPtr resource;
            int levels = mips ? 0 : 1;
            NativeMethods.LoadTextureFromFile(device.Device.NativePointer, path, out resource, levels);

            if (resource != IntPtr.Zero)
            {
                Texture2D texture = Texture2D.FromPointer<Texture2D>(resource);
                
                if (texture.Description.ArraySize == 6 && texture.Description.OptionFlags.HasFlag(ResourceOptionFlags.TextureCube))
                {
                    return new DX11TextureCube(device, texture);
                }
                else
                {
                    texture.Dispose();
                    throw new Exception("This texture is not a cube texture");
                }
            }
            else
            {
                throw new Exception("Failed to load texture");
            }
        }


        public static DX11TextureArray2D LoadTextureArrayFromFile(DxDevice device, string path, bool mips = true)
        {
            IntPtr resource;
            int levels = mips ? 0 : 1;
            NativeMethods.LoadTextureFromFile(device.Device.NativePointer, path, out resource, levels);

            if (resource != IntPtr.Zero)
            {
                Texture2D texture = Texture2D.FromPointer<Texture2D>(resource);

                if (texture.Description.ArraySize > 1)
                {
                    return new DX11TextureArray2D(device, texture);
                }
                else
                {
                    texture.Dispose();
                    throw new Exception("This texture is not a texture array");
                }
            }
            else
            {
                throw new Exception("Failed to load texture");
            }
        }

        public static DX11Texture3D LoadTexture3D(DxDevice device, string path)
        {
            IntPtr resource;
            long retcode = NativeMethods.LoadTextureFromFile(device.Device.NativePointer, path, out resource, 1);

            if (retcode == 0)
            {
                Resource r = Resource.FromPointer<Resource>(resource);
                if (r.Dimension != ResourceDimension.Texture3D)
                {
                    r.Dispose();
                    throw new Exception("Texture is not a 3D Texture");
                }

                Texture3D texture = Texture3D.FromPointer<Texture3D>(resource);
                ShaderResourceView srv = new ShaderResourceView(device, texture);
                return DX11Texture3D.FromReference(device, texture, srv);
            }
            else
            {
                throw new Exception("Failed to load texture");
            }
        }

        public static DX11Texture1D LoadTexture1D(DxDevice device, string path)
        {
            IntPtr resource;
            long retcode = NativeMethods.LoadTextureFromFile(device.Device.NativePointer, path, out resource, 1);

            if (retcode == 0)
            {
                Resource r = Resource.FromPointer<Resource>(resource);
                if (r.Dimension != ResourceDimension.Texture1D)
                {
                    r.Dispose();
                    throw new Exception("Texture is not a 1D Texture");
                }

                Texture1D texture = Texture1D.FromPointer<Texture1D>(resource);
                ShaderResourceView srv = new ShaderResourceView(device, texture);
                return DX11Texture1D.FromReference(device, texture, srv);
            }
            else
            {
                throw new Exception("Failed to load texture");
            }
        }


        private static eImageFormat GetCodec(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".bmp":
                    return eImageFormat.Bmp;
                case ".dds":
                    return eImageFormat.Dds;
                case ".tga":
                    return eImageFormat.Tga;
                case ".jpg":
                    return eImageFormat.Jpeg;
                case ".jpeg":
                    return eImageFormat.Jpeg;
                case ".png":
                    return eImageFormat.Png;
                case ".tif":
                    return eImageFormat.Tiff;
                case ".tiff":
                    return eImageFormat.Tiff;
                case ".wdp":
                    return eImageFormat.Hdp;
                case ".hdp":
                    return eImageFormat.Hdp;
                default:
                    return eImageFormat.Dds;
            }
        }

        public static void SaveToFile(DxDevice device, RenderContext context,IDxTexture2D texture, string path)
        {
            eImageFormat format = GetCodec(path);
            long retcode = NativeMethods.SaveTextureToFile(device.Device.NativePointer, context.Context.NativePointer, texture.Texture.NativePointer, path, (int)format);

            if (retcode != 0)
            {
                throw new Exception("Failed to Save Texture");
            }
        }

        public static void SaveToFileCompressed(DxDevice device, RenderContext context, IDxTexture2D texture, string path,DdsBlockType blockType)
        {
            long retcode = NativeMethods.SaveCompressedTextureToFile(device.Device.NativePointer, context.Context.NativePointer, 
                texture.Texture.NativePointer, path, (int)blockType);

            if (retcode != 0)
            {
                throw new Exception("Failed to Save Texture");
            }
        }

        public static void SaveToFile(DxDevice device, RenderContext context, IDxTexture3D texture, string path)
        {
            eImageFormat format = eImageFormat.Dds;
            long retcode = NativeMethods.SaveTextureToFile(device.Device.NativePointer, context.Context.NativePointer, texture.Texture.NativePointer, path, (int)format);

            if (retcode != 0)
            {
                throw new Exception("Failed to Save Texture");
            }
        }

        public static void SaveToFile(DxDevice device, RenderContext context, IDxTexture1D texture, string path)
        {
            eImageFormat format = eImageFormat.Dds;
            long retcode = NativeMethods.SaveTextureToFile(device.Device.NativePointer, context.Context.NativePointer, texture.Texture.NativePointer, path, (int)format);

            if (retcode != 0)
            {
                throw new Exception("Failed to Save Texture");
            }
        }
       
    }
}
