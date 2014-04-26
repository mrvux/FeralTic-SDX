using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Resources
{
    public class DX11TextureArray2D : DX11Texture2D
    {
        public int ArraySize { get { return this.Texture.Description.ArraySize; } }


        public DX11TextureArray2D(DxDevice device, Texture2D texture)
        {
            this.Texture = texture;
            this.description = this.Texture.Description;

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                Format = this.Texture.Description.Format,
                Texture2DArray = new ShaderResourceViewDescription.Texture2DArrayResource()
                {
                    ArraySize = this.Texture.Description.ArraySize,
                    FirstArraySlice = 0,
                    MipLevels = this.Texture.Description.MipLevels,
                    MostDetailedMip = 0
                }
            };

            this.ShaderView = new ShaderResourceView(device, this.Texture, srvd);

        }
    }
}
