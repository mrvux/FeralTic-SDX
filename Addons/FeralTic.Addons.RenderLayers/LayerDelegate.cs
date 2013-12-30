using FeralTic.DX11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers
{
    public delegate void RenderDelegate<S ,T>(S source, T settings);

    public class DX11BaseLayer<S ,T> : IDxResource
    {
        public RenderDelegate<S, T> Render;

        public void Dispose()
        {

        }
    }
}
