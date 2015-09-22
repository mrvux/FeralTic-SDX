using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Box : AbstractPrimitiveDescriptor , IEquatable<Box>
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
        
        public Box()
        {
            this.size = new Vector3(1, 1, 1);
        }

        public override string PrimitiveType { get { return "Box"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Box(this);
        }

        public bool Equals(Box obj)
        {
            return this.Size == obj.Size;
        }
    }

    public class BoxLine : Box,  IEquatable<BoxLine>
    {
        public override string PrimitiveType { get { return "BoxLine"; } }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.BoxLine(this);
        }

        public bool Equals(BoxLine o)
        {
            return this.Size == o.Size;
        }
    }
}
