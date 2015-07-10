using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Dodecahedron : AbstractPrimitiveDescriptor, IEquatable<Dodecahedron>
    {
        private Vector3 size;

        public Vector3 Size
        {
            get { return this.size; }
            set
            {
                if (this.size != value)
                {
                    this.size = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Dodecahedron()
        {
            this.Size = new Vector3(1, 1, 1);
        }

        public override string PrimitiveType { get { return "Dodecahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Dodecahedron(this);
        }

        public bool Equals(Dodecahedron other)
        {
            return this.Size == other.Size;
        }
    }
}
