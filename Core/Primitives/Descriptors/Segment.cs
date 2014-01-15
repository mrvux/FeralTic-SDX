using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Segment : AbstractPrimitiveDescriptor
    {
        public Segment()
        {
            this.Phase = 0.0f;
            this.Cycles = 1.0f;
            this.InnerRadius = 0.0f;
            this.Flat = true;
            this.Resolution = 20;
        }

        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public bool Flat { get; set; }
        public int Resolution { get; set; }

        public override string PrimitiveType { get { return "Segment"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Phase = (float)properties["Phase"];
            this.Cycles = (float)properties["Cycles"];
            this.InnerRadius = (float)properties["InnerRadius"];
            this.Resolution = (int)properties["Resolution"];
            this.Flat = (bool)properties["Flat"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Segment(this);
        }
    }
}
