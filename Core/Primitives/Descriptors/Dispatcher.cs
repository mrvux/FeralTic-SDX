using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Dispatcher : AbstractPrimitiveDescriptor
    {
        public Dispatcher()
        {
            this.ThreadCountX = 1;
            this.ThreadCountY = 1;
            this.ThreadCountZ = 1;
        }

        public int ThreadCountX { get; set; }
        public int ThreadCountY { get; set; }
        public int ThreadCountZ { get; set; }

        public override string PrimitiveType { get { return "Dispatcher"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Dispatcher(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Dispatcher))
            {
                return false;
            }
            Dispatcher o = (Dispatcher)obj;
            return this.ThreadCountX == o.ThreadCountX
                && this.ThreadCountY == o.ThreadCountY
                && this.ThreadCountZ == o.ThreadCountZ;
        }
    }
}
