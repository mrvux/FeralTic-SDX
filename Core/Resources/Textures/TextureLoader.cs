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

    public class TextureLoader
    {
        private class NativeMethods
        {
            [DllImport("DirectXTexLib_x86",CharSet=CharSet.Unicode)]
            public static extern long LoadTextureFromFile(IntPtr device, string path,out IntPtr resource, int miplevels);

            [DllImport("DirectXTexLib_x86", CharSet = CharSet.Unicode)]
            public static extern long SaveTextureToFile(IntPtr device,IntPtr context,IntPtr resource, string path,int format);

            [DllImport("DirectXTexLib_x86", CharSet = CharSet.Unicode)]
            public static extern long SaveBC7TextureToFile(IntPtr device, IntPtr context, IntPtr resource, string path);
        }

        public static DX11Texture2D LoadFromFile(DxDevice device, string path, bool mips = true)
        {
            IntPtr resource;
            int levels = mips ? 0 : 1;
            long retcode = NativeMethods.LoadTextureFromFile(device.Device.NativePointer, path, out resource, levels);

            if (retcode == 0)
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

        public static void SaveToFileBC7(DxDevice device, RenderContext context, IDxTexture2D texture, string path)
        {
            long retcode = NativeMethods.SaveBC7TextureToFile(device.Device.NativePointer, context.Context.NativePointer, texture.Texture.NativePointer, path);

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
