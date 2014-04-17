using FeralTic.DX11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Materials
{
    public interface IShaderMaterial
    {
        void Attach(RenderContext context);

        void Detach(RenderContext context);
    }
}
