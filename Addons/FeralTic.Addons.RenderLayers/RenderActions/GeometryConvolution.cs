using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FeralTic.DX11;

namespace FeralTic.RenderLayers
{
    public abstract class GeometryConvolution<I, O>
        where I : IDxGeometry
        where O : IDxGeometry
    {
        public abstract O Convolute(I input);
    }
}
