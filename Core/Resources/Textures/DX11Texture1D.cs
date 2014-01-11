using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using SharpDX.Direct3D11;
using SharpDX;

namespace FeralTic.DX11.Resources
{
    public class DX11Texture1D : IDxTexture1D, IDxUnorderedResource
    {
        private DxDevice device;

        public Texture1D Texture { get; private set; }

        public ShaderResourceView ShaderView { get; private set; }
        public UnorderedAccessView UnorderedView { get; private set; }

        private Texture1DDescription description;

        public int Width 
        { 
            get { return this.description.Width; }        
        }

        public SharpDX.DXGI.Format Format
        {
            get { return this.description.Format; }
        }

        protected DX11Texture1D(DxDevice device)
        {
            this.device = device;
        }

        protected DX11Texture1D(DxDevice device, Texture1DDescription desc)
        {
            this.device = device;
            this.description = desc;
            this.Texture = new Texture1D(device.Device, desc);
            this.ShaderView = new ShaderResourceView(device.Device, this.Texture);

            if (desc.BindFlags.HasFlag(BindFlags.UnorderedAccess))
            {
                this.UnorderedView = new UnorderedAccessView(device.Device, this.Texture);
            }
        }

        public DataStream MapForWrite(RenderContext context)
        {
            DataStream ds;
            context.Context.MapSubresource(this.Texture, 0, MapMode.WriteDiscard, MapFlags.None, out ds);
            return ds;
        }

        public void Unmap(RenderContext context)
        {
            context.Context.UnmapSubresource(this.Texture, 0);
        }

        public void WriteData<T>(RenderContext context, T[] data) where T : struct
        {
            DataStream ds = this.MapForWrite(context);
            ds.WriteRange<T>(data);
            this.Unmap(context);
        }

        public static DX11Texture1D FromReference(DxDevice device, Texture1D texture, ShaderResourceView view)
        {
            DX11Texture1D result = new DX11Texture1D(device);
            result.description = texture.Description;
            result.ShaderView = view;
            result.Texture = texture;
            return result;
        }

        public static DX11Texture1D CreateDynamic(DxDevice device, int width, SharpDX.DXGI.Format format)
        {
            Texture1DDescription desc = new Texture1DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                Width = width,
                Format = format,
                MipLevels = 1,
                Usage = ResourceUsage.Dynamic
            };
            return new DX11Texture1D(device, desc);
        }

        public static DX11Texture1D CreateWriteable(DxDevice device, int width, SharpDX.DXGI.Format format)
        {
            Texture1DDescription desc = new Texture1DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Width = width,
                MipLevels = 1,
                Format = format,
                Usage = ResourceUsage.Default
            };
            return new DX11Texture1D(device, desc);
        }

        public void Dispose()
        {
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
        }
    }
}
