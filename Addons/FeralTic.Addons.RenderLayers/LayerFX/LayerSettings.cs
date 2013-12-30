using FeralTic.DX11;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public partial class LayerSettings : IViewProjectionSettings
    {
        public LayerSettings()
        {
            this.TransformIn = Matrix.Identity;
            this.HasTransformIn = false;
            this.TransformOut = Matrix.Identity;
            this.HasTransformOut = false;
            this.View = Matrix.Identity;
            this.Projection = Matrix.Identity;
            this.Aspect = Matrix.Identity;
            this.Crop = Matrix.Identity;
            this.ViewProjection = Matrix.Identity;

            this.ObjectValidators = new List<IObjectLayerValidator>();

            this.ViewportCount = 1;
            this.ViewportIndex = 0;
            this.RenderHint = eRenderHint.Forward;
        }

        public eRenderHint RenderHint { get; set; }

        public RenderContext RenderContext { get; set; }

        public int RenderWidth { get; set; }
        public int RenderHeight { get; set; }
        public int RenderDepth { get; set; }

        public int ViewportIndex { get; set; }
        public int ViewportCount { get; set; }

        public IDxUnorderedResource BackBuffer { get; set; }
        public IDxShaderResource ReadBuffer { get; set; }
        public IDxGeometry Geometry { get; set; }

        public bool ResetCounter { get; set; }
        public int CounterValue { get; set; }

        public bool DepthOnly { get; set; }

        public List<IObjectLayerValidator> ObjectValidators { get; set; }

        public bool ValidateObject(ObjectLayerSettings obj)
        {
            foreach (IObjectLayerValidator objval in this.ObjectValidators)
            {
                if (objval.Enabled)
                {
                    if (!objval.Validate(obj)) { return false; }
                }
            }
            return true;
        }
    }
}
