using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Sphere : AbstractPrimitiveDescriptor
    {
        public Sphere()
        {
            this.Radius = 0.5f;
            this.CyclesX = 1.0f;
            this.CyclesY = 1.0f;
            this.ResX = 15;
            this.ResY = 15;
        }

        public float Radius { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }
        public float CyclesX { get; set; }
        public float CyclesY { get; set; }

        public override string PrimitiveType { get { return "Sphere"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.ResX = (int)properties["ResX"];
            this.ResY = (int)properties["ResY"];
            this.CyclesX = (float)properties["CyclesX"];
            this.CyclesY = (float)properties["CyclesY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Sphere(this);
        }
    }

}
