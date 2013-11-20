using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11SliceRenderTarget : IDX11RenderTarget, IDisposable
    {
        private IDX11Texture2D parent;
        private DX11Device device;

        public RenderTargetView RenderView { get; protected set; }

        public int Width { get { return this.parent.Width; } }
        public int Height { get { return this.parent.Height; } }

        public DX11SliceRenderTarget(DX11Device device, IDX11Texture2D texture, int sliceindex)
        {
            this.device = device;
            this.parent = texture;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture2DArray,
                Format = texture.Format,
                Texture2DArray = new RenderTargetViewDescription.Texture2DArrayResource()
                {
                    ArraySize = 1,
                    MipSlice = 0,
                    FirstArraySlice = sliceindex
                }
            };

            this.RenderView = new RenderTargetView(device.Device, this.parent.Texture, rtd);
        }

        public void Dispose()
        {
            this.RenderView.Dispose();
        }
    }
}
