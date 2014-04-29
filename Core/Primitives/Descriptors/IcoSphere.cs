using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class IcoSphere : AbstractPrimitiveDescriptor
    {
        public IcoSphere()
        {
            this.Radius = 0.5f;
            this.SubDivisions = 1;
        }

        public float Radius { get; set; }
        public int SubDivisions { get; set; }

        public override string PrimitiveType { get { return "IcoSphere"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.SubDivisions = (int)properties["SubDivisions"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.IcoSphere(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IcoSphere))
            {
                return false;
            }
            IcoSphere o = (IcoSphere)obj;
            return this.Radius == o.Radius
                && this.SubDivisions == o.SubDivisions;
        }
    }

}
