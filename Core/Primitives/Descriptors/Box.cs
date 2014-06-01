using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Box : AbstractPrimitiveDescriptor
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
