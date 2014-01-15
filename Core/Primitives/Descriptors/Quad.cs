using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Quad : AbstractPrimitiveDescriptor
    {
        public Quad()
        {
            this.Size = new Vector2(1, 1);
        }

        public Vector2 Size { get; set; }

        public override string PrimitiveType { get { return "Quad"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.QuadNormals(this);
        }
    }

    public class QuadLine : Quad
    {
        public override string PrimitiveType { get { return "QuadLine"; } }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.QuadLine(this);
        }
    }

}
