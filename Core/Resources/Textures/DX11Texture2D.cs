using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;
using System.Runtime.InteropServices;

namespace FeralTic.DX11.Resources
{
    public unsafe class DX11Texture2D : IDxTexture2D , IDisposable
    {
        [DllImport("msvcrt.dll", SetLastError = false)]
        static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        [DllImport("msvcrt.dll", SetLastError = false)]
        static extern IntPtr memcpybyte(byte* dest, byte* src, int count);

        public Texture2D Texture { get; private set; }
        public ShaderResourceView ShaderView { get; private set; }
        private Texture2DDescription description;

        public int Width 
        { 
            get { return this.description.Width; }        
        }

        public int Height
        {
            get { return this.description.Height; }
        }

        public Format Format
        {
            get { return this.description.Format; }
        }

        public static DX11Texture2D FromFile(DxDevice device, string path, bool mips = true)
        {
            DX11Texture2D texture = new DX11Texture2D();
            texture.Texture = FileLoader.CreateTexture2DFromBitmap(device.Device, FileLoader.LoadBitmap(device.WICFactory, path), mips);
            texture.description = texture.Texture.Description;
            texture.ShaderView = new ShaderResourceView(device.Device, texture.Texture);
            if (mips) { device.Device.ImmediateContext.GenerateMips(texture.ShaderView); }
            return texture;
        }

        public static DX11Texture2D FromReference(DxDevice device, Texture2D texture, ShaderResourceView view)
        {
            DX11Texture2D result = new DX11Texture2D();
            result.description = texture.Description;
            result.ShaderView = view;
            result.Texture = texture;
            return result;
        }

        public static DX11Texture2D CreateDynamic(DxDevice device, int width, int height, Format format)
        {
            //device.ComPointer.
            var desc = new Texture2DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = format,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Dynamic,
                Width = width,
                Height = height,
                SampleDescription = new SampleDescription(1, 0)
            };

            DX11Texture2D texture = new DX11Texture2D();
            texture.Texture = new Texture2D(device.Device, desc);
            texture.description = texture.Texture.Description;
            texture.ShaderView = new ShaderResourceView(device.Device,texture.Texture);
            return texture;
            
        }

        public unsafe void WriteData(RenderContext context, IntPtr ptr, int len, int pixelsize)
        {
            DeviceContext ctx = context;
            DataStream ds;
            DataBox db = ctx.MapSubresource(this.Texture, 0, 0, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out ds);

            int stride = pixelsize * this.Width;

            if (stride != db.RowPitch)
            {
                byte* bsource = (byte*)ptr.ToPointer();
                byte* bdest = (byte*)ptr.ToPointer();
                try
                {
                    //Row per row copy
                    for (int i = 0; i < this.Height; i++)
                    {
                        memcpybyte(bdest,bsource,stride);
                        bdest += db.RowPitch;
                        bsource += stride;
                    }
                }
                catch { }
            }
            else
            {
                memcpy(db.DataPointer, ptr, len);
            }
            ctx.UnmapSubresource(this.Texture, 0);
        }


        public void Dispose()
        {
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
        }
    }
}
