using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Cylinder : AbstractPrimitiveDescriptor
    {
        public Cylinder()
        {
            this.Radius1 = 0.5f;
            this.Radius2 = 0.5f;
            this.Length = 1.0f;
            this.Cycles = 1.0f;
            this.Caps = true;
            this.ResolutionX = 15;
            this.ResolutionY = 1;
        }

        public float Radius1 { get; set; }
        public float Radius2 { get; set; }
        public float Cycles { get; set; }
        public float Length { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public bool Caps { get; set; }

        public override string PrimitiveType { get { return "Cylinder"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius1 = (float)properties["Radius1"];
            this.Radius2 = (float)properties["Radius2"];
            this.Cycles = (float)properties["Cycles"];
            this.Length = (float)properties["Length"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
            this.Caps = (bool)properties["Caps"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Cylinder(this);
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Cylinder))
            {
                return false;
            }
            Cylinder o = (Cylinder)obj;
            return this.Caps == o.Caps
                && this.Cycles == o.Cycles
                && this.Length == o.Length
                && this.Radius1 == o.Radius1
                && this.Radius2 == o.Radius2
                && this.ResolutionX == o.ResolutionX
                && this.ResolutionY == o.ResolutionY;
        }
    }


}
