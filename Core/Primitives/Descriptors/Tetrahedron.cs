using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Tetrahedron : AbstractPrimitiveDescriptor
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

        public Tetrahedron()
        {
            this.Size = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public override string PrimitiveType { get { return "Tetrahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Tetrahedron(this);
        }
    }
}
