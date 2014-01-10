using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Resources
{
    public class DxMipSliceRenderTarget : IDxRenderTarget, IDisposable
    {
        private DxDevice device;

        public RenderTargetView RenderView { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }

        public DxMipSliceRenderTarget(DxDevice device, IDxTexture2D texture, int mipindex, int w, int h)
        {
            this.device = device;
            this.Width = w;
            this.Height = h;
            this.Depth = 1;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture2D,
                Format = texture.Format,
                Texture2D = new RenderTargetViewDescription.Texture2DResource()
                {
                    MipSlice = mipindex
                }
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture2D,
                Format = texture.Format,
                Texture2D = new ShaderResourceViewDescription.Texture2DResource()
                {
                    MipLevels = 1,
                    MostDetailedMip = mipindex
                }
            };

            this.RenderView = new RenderTargetView(device, texture.Texture, rtd);
            this.ShaderView = new ShaderResourceView(device, texture.Texture, srvd);
            
        }

        public DxMipSliceRenderTarget(DxDevice device, IDxTexture3D texture, int mipindex, int w, int h, int d)
        {
            this.device = device;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture3D,
                Format = texture.Format,
                Texture3D = new RenderTargetViewDescription.Texture3DResource()
                {
                    MipSlice = mipindex
                }
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture3D,
                Format = texture.Format,
                Texture3D = new ShaderResourceViewDescription.Texture3DResource()
                {
                    MipLevels = 1,
                    MostDetailedMip = mipindex
                }
            };

            this.RenderView = new RenderTargetView(device.Device, texture.Texture, rtd);
            this.ShaderView = new ShaderResourceView(device.Device, texture.Texture, srvd);

            this.Width = w;
            this.Height = h;
            this.Depth = d;
        }

        public void Dispose()
        {
            this.RenderView.Dispose();
            this.ShaderView.Dispose();
        }
    }
}
