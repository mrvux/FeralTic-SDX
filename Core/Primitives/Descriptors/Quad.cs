using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Quad : AbstractPrimitiveDescriptor, IEquatable<Quad>
    {
        private Vector2 size;

        public Quad()
        {
            this.size = new Vector2(1, 1);
        }

        public Vector2 Size
        {
            get { return this.size; }
            set
            {
                bool changed = this.size != value;
                this.size = value;
                if (changed) { this.RaisePropertyChanged(); }
            }
        }

        public override string PrimitiveType { get { return "Quad"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.QuadNormals(this);
        }

        public bool Equals(Quad o)
        {
            return this.Size == o.Size;
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
