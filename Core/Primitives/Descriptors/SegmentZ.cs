using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class SegmentZ : AbstractPrimitiveDescriptor
    {
        public SegmentZ()
        {
            this.Phase = 0.0f;
            this.Cycles = 1.0f;
            this.InnerRadius = 0.0f;
            this.Z = 0.5f;
            this.Resolution = 20;
        }

        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public float Z { get; set; }
        public int Resolution { get; set; }

        public override string PrimitiveType { get { return "SegmentZ"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Phase = (float)properties["Phase"];
            this.Cycles = (float)properties["Cycles"];
            this.InnerRadius = (float)properties["InnerRadius"];
            this.Resolution = (int)properties["Resolution"];
            this.Z = (float)properties["Z"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.SegmentZ(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SegmentZ))
            {
                return false;
            }
            SegmentZ o = (SegmentZ)obj;
            return this.Cycles == o.Cycles
                && this.InnerRadius == o.InnerRadius
                && this.Phase == o.Phase
                && this.Resolution == o.Resolution
                && this.Z == o.Z;
        }
    }
}
