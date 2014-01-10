using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DxRenderMip3D : IDxTexture3D
    {
        public RenderTargetView RenderView { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public Texture3D Texture { get; protected set; }

        private Texture3DDescription resourceDesc;

        public int Width
        {
            get { return resourceDesc.Width; }
        }

        public int Height
        {
            get { return resourceDesc.Height; }
        }

        public int Depth
        {
            get { return resourceDesc.Depth; }
        }

        public Format Format
        {
            get { return resourceDesc.Format; }
        }

        public DxMipSliceRenderTarget[] Slices { get; protected set; }

        private int CountMipLevels(int w,int h,int d)
        {
            int level = 1;
            while (w > 1 && h > 1 && d > 1)
            {
                w /= 2; h /= 2; d /= 2; level++;
            }
            return level;
        }

        public DxRenderMip3D(DxDevice device, int w, int h, int d, Format format)
        {
            int levels = this.CountMipLevels(w,h,d);
            var texBufferDesc = new Texture3DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                Depth = d,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default,
                MipLevels = levels,
            };

            this.Texture = new Texture3D(device, texBufferDesc);
            this.resourceDesc = this.Texture.Description;

            this.ShaderView = new ShaderResourceView(device, this.Texture);
            this.RenderView = new RenderTargetView(device, this.Texture);

            this.Slices = new DxMipSliceRenderTarget[levels];

            int sw = w;
            int sh = h;
            int sd = d;

            for (int i = 0; i < levels; i++)
            {
                this.Slices[i] = new DxMipSliceRenderTarget(device, this, i, w, h,d);
                w /= 2; h /= 2; d /= 2;
            }
        }

        public void Dispose()
        {
            if (this.RenderView != null) { this.RenderView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
            foreach (DxMipSliceRenderTarget slice in this.Slices)
            {
                slice.Dispose();
            }
        }
    }
}
