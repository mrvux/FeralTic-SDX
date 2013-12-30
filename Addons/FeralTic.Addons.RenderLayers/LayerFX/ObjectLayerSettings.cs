using FeralTic.DX11;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public class ObjectLayerSettings
    {
        public int DrawCallIndex { get; set; }
        public Matrix WorldTransform { get; set; }
        public IDxGeometry Geometry { get; set; }
    }
}
