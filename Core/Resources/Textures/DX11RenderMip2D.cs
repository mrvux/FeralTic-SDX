using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DxRenderMip2D : IDxTexture2D
    {
        public RenderTargetView RenderView { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public Texture2D Texture { get; protected set; }

        private Texture2DDescription resourceDesc;

        public int Width
        {
            get { return resourceDesc.Width; }
        }

        public int Height
        {
            get { return resourceDesc.Height; }
        }

        public Format Format
        {
            get { return resourceDesc.Format; }
        }

        public DxMipSliceRenderTarget[] Slices { get; protected set; }

        private int CountMipLevels(int w,int h)
        {
            int level = 1;
            while (w > 1 && h > 1)
            {
                w /= 2; h /= 2; level++;
            }
            return level;
        }

        public DxRenderMip2D(DxDevice device, int w, int h, Format format)
        {
            int levels = this.CountMipLevels(w,h);
            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                MipLevels = levels,
            };

            this.Texture = new Texture2D(device, texBufferDesc);
            this.resourceDesc = this.Texture.Description;

            this.RenderView = new RenderTargetView(device, this.Texture);
            this.ShaderView = new ShaderResourceView(device, this.Texture);

            this.Slices = new DxMipSliceRenderTarget[levels];

            int sw = w;
            int sh = h;

            for (int i = 0; i < levels; i++)
            {
                this.Slices[i] = new DxMipSliceRenderTarget(device, this, i, w, h);
                w /= 2; h /= 2;
            }
        }

        public void Dispose()
        {
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.RenderView != null) { this.RenderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
            foreach (DxMipSliceRenderTarget slice in this.Slices)
            {
                slice.Dispose();
            }
        }
    }
}
