using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;

namespace FeralTic.DX11.Resources
{
    public class DX11RenderTexture3D : IDX11Texture3D, IDX11RenderTarget, IDX11UnorderedResource
    {
        private DX11Device device;

        public ShaderResourceView ShaderView { get; protected set; }
        public UnorderedAccessView UnorderedView { get; protected set; }
        public RenderTargetView RenderView { get; protected set; }

        public Texture3D Texture { get; protected set; }
        private Texture3DDescription resourceDesc;

        public SharpDX.DXGI.Format Format { get { return resourceDesc.Format; } }
        public int Width { get { return resourceDesc.Width; } }
        public int Height { get { return resourceDesc.Height; } }
        public int Depth { get { return resourceDesc.Depth; } }
       
        public DX11RenderTexture3D(DX11Device device, int w, int h, int d, Format format)
        {
            this.device = device;
            Texture3DDescription desc = new Texture3DDescription()
            {
                BindFlags = BindFlags.UnorderedAccess | BindFlags.ShaderResource | BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                Depth = d,
                Format = format,
                Height = h,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default,
                Width = w
            };

            this.Texture = new Texture3D(device.Device, desc);
            this.resourceDesc = this.Texture.Description;
            this.ShaderView = new ShaderResourceView(device.Device, this.Texture);
            this.UnorderedView = new UnorderedAccessView(device.Device, this.Texture);
            this.RenderView = new RenderTargetView(device.Device, this.Texture);
        }

        public void Clear(DX11RenderContext context, Color4 color)
        {
            context.Context.ClearRenderTargetView(this.RenderView, color);
        }

        public void  Dispose()
        {
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.RenderView != null) { this.RenderView.Dispose(); }
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
        }
    }
}
