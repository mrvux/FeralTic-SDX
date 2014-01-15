using SharpDX;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class NullGeometry : AbstractPrimitiveDescriptor
    {
        public NullGeometry()
        {
            this.VertexCount = 1;
            this.InstanceCount = 1;
            this.Topology = PrimitiveTopology.PointList;
        }

        public int VertexCount { get; set; }
        public int InstanceCount { get; set; }
        public PrimitiveTopology Topology { get; set; }

        public override string PrimitiveType { get { return "Null"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.VertexCount = (int)properties["VertexCount"];
            this.InstanceCount = (int)properties["InstanceCount"];
            this.Topology = (PrimitiveTopology)properties["Topology"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.NullDrawer(this);
        }
    }

}
