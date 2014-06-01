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
        private int vertexCount;
        private int instanceCount;
        private PrimitiveTopology topology;

        public int VertexCount
        {
            get { return this.vertexCount; }
            set
            {
                if (vertexCount != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Vertex Count must be positive or 0", "VertexCount");
                    }
                    this.vertexCount = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int InstanceCount
        {
            get { return this.instanceCount; }
            set
            {
                if (instanceCount != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Instance Count must be positive or 0", "InstanceCount");
                    }
                    this.instanceCount = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public PrimitiveTopology Topology
        {
            get { return this.topology; }
            set
            {
                if (this.topology != value)
                {
                    this.topology = value;
                    this.RaisePropertyChanged();
                }
            }

        }

        public NullGeometry()
        {
            this.VertexCount = 1;
            this.InstanceCount = 1;
            this.Topology = PrimitiveTopology.PointList;
        }



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
