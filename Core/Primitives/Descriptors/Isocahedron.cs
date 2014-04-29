using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Isocahedron : AbstractPrimitiveDescriptor
    {
        public Isocahedron()
        {
            this.Size = new Vector3(1, 1, 1);
        }

        public Vector3 Size { get; set; }

        public override string PrimitiveType { get { return "Isocahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Isocahedron(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Isocahedron))
            {
                return false;
            }
            Isocahedron o = (Isocahedron)obj;
            return this.Size == o.Size;
        }
    }
}
