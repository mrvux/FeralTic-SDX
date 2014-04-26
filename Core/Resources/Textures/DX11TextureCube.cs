using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Resources
{
    public class DX11TextureCube : DX11Texture2D
    {
        public DX11TextureCube(DxDevice device, Texture2D texture)
        {
            this.Texture = texture;
            this.description = this.Texture.Description;

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.TextureCube,
                Format= this.Texture.Description.Format,
                TextureCube = new ShaderResourceViewDescription.TextureCubeResource()
                {
                    MipLevels = this.Texture.Description.MipLevels,
                    MostDetailedMip = 0
                }
            };

            this.ShaderView = new ShaderResourceView(device, this.Texture, srvd);

        }
    }
}
