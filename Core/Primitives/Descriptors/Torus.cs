using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Torus : AbstractPrimitiveDescriptor
    {
        public Torus()
        {
            this.Radius = 0.5f;
            this.Thickness = 0.1f;
            this.ResolutionX = 15;
            this.ResolutionY = 15;
            this.PhaseX = 1.0f;
            this.PhaseY = 1.0f;
            this.Rotation = 1.0f;
            this.CY = 1.0f;
        }

        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public float Radius { get; set; }
        public float Thickness { get; set; }
        public float PhaseY { get; set; }
        public float PhaseX { get; set; }
        public float Rotation { get; set; }
        public float CY { get; set; }

        public override string PrimitiveType { get { return "Torus"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];

            this.Thickness = (float)properties["Thickness"];

            this.PhaseY = (float)properties["PhaseY"];
            this.PhaseX = (float)properties["PhaseX"];
            this.Rotation = (float)properties["Rotation"];
            this.CY = (float)properties["CY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Torus(this);
        }
    }

}
