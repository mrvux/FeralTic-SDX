using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Box : AbstractPrimitiveDescriptor
    {
        public Box()
        {
            this.Size = new Vector3(1, 1, 1);
        }

        public Vector3 Size { get; set; }

        public override string PrimitiveType { get { return "Box"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Box(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Box))
            {
                return false;
            }
            Box box = (Box)obj;
            return this.Size == box.Size;
        }
    }

    public class BoxLine : Box
    {
        public override string PrimitiveType { get { return "BoxLine"; } }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.BoxLine(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoxLine))
            {
                return false;
            }
            BoxLine box = (BoxLine)obj;
            return this.Size == box.Size;
        }
    }
}
