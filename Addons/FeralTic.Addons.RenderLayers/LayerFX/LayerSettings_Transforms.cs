using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public partial class LayerSettings
    {
        public bool HasTransformIn { get; set; }
        public Matrix TransformIn { get; set; }

        public bool HasTransformOut { get; set; }
        public Matrix TransformOut { get; set; }

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Matrix ViewProjection { get; set; }
        public Matrix Aspect { get; set; }
        public Matrix Crop { get; set; }
    }
}
