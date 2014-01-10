using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DX11SliceDepthStencil : IDisposable
    {
        private IDxTexture2D parent;
        private DxDevice device;

        public DepthStencilView DSV { get; protected set; }
        public DepthStencilView ReadOnlyDSV { get { return null; } }

        public int Width { get { return this.parent.Width; } }
        public int Height { get { return this.parent.Height; } }

        public DX11SliceDepthStencil(DxDevice device, IDxTexture2D texture, int sliceindex, eDepthFormat depthformat)
        {
            this.device = device;
            this.parent = texture;

            DepthStencilViewDescription dsvd = new DepthStencilViewDescription()
            {
                Format = depthformat.GetDepthFormat(),
                Dimension = DepthStencilViewDimension.Texture2DArray,
                 Texture2DArray = new DepthStencilViewDescription.Texture2DArrayResource()
                 {
                     ArraySize = 1,
                     FirstArraySlice = sliceindex,
                     MipSlice = 0
                 }
            };

            this.DSV = new DepthStencilView(device, this.parent.Texture, dsvd);
        }

        public void Dispose()
        {
            this.DSV.Dispose();
        }



    }
}
