using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Octahedron : AbstractPrimitiveDescriptor
    {
        public Octahedron()
        {
            this.Size = new Vector3(1, 1, 1);
        }

        public Vector3 Size { get; set; }

        public override string PrimitiveType { get { return "Octahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Octahedron(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Octahedron))
            {
                return false;
            }
            Octahedron o = (Octahedron)obj;
            return this.Size == o.Size;
        }
    }

}
