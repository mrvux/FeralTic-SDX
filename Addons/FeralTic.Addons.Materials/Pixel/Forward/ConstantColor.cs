using FeralTic.DX11;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Materials.Pixel.Forward
{
    public class ConstantColor : IShaderMaterial
    {
        private PixelShader shader;

        public Color4 Color { get; set; }

        public void Attach(RenderContext context)
        {
            context.Context.PixelShader.Set(this.shader);
        }

        public void Detach(RenderContext context)
        {
            context.Context.PixelShader.Set(null);
        }

        public void Update(RenderContext context)
        {

        }
    }
}
