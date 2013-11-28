using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public class DefaultTextures : IDisposable
    {
        private DxDevice device;

        private IDxTexture2D blacktex;
        private IDxTexture2D whitetex;

        public DefaultTextures(DxDevice device)
        {
            this.device = device;
        }

        public IDxTexture2D BlackTexture
        {
            get
            {
                if (this.blacktex == null)
                {
                    blacktex = TextureUtils.SolidColor(device, SharpDX.Color4.Black);
                }
                return blacktex;
            }
        }

        public IDxTexture2D WhiteTexture
        {
            get
            {
                if (this.whitetex == null)
                {
                    whitetex = TextureUtils.SolidColor(device, SharpDX.Color4.White);
                }
                return whitetex;
            }
        }

        public void Dispose()
        {
            if (this.blacktex != null) { this.blacktex.Dispose(); this.blacktex = null; }
            if (this.whitetex != null) { this.whitetex.Dispose(); this.whitetex = null; }
        }

    }
}
