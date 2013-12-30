using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FeralTic.DX11.Resources;
using FeralTic.DX11;
using SharpDX.Direct3D;
using SharpDX;

namespace FeralTic.RenderLayers
{
    public class IndexedInstancer : GeometryConvolution<DX11IndexedGeometry, DX11IndexedGeometry>
    {
        public int InstanceCount { get; set; }

        public IndexedInstancer()
        {
            this.InstanceCount = 1;
        }

        public override DX11IndexedGeometry Convolute(DX11IndexedGeometry input)
        {
            DX11IndexedGeometry result = (DX11IndexedGeometry)input.ShallowCopy();
            DX11InstancedIndexedDrawer d = new DX11InstancedIndexedDrawer();
            d.InstanceCount = this.InstanceCount;
            return result;
        }
    }

    public class VertexInstancer : GeometryConvolution<DX11VertexGeometry, DX11VertexGeometry>
    {
        public int InstanceCount { get; set; }

        public VertexInstancer()
        {
            this.InstanceCount = 1;
        }

        public override DX11VertexGeometry Convolute(DX11VertexGeometry input)
        {
            DX11VertexGeometry result = (DX11VertexGeometry)input.ShallowCopy();
            DX11InstancedVertexDrawer d = new DX11InstancedVertexDrawer();
            d.InstanceCount = this.InstanceCount;
            return result;
        }
    }

    public class Instancer : GeometryConvolution<IDxGeometry, IDxGeometry>
    {
        public int InstanceCount { get; set; }

        public Instancer()
        {
            this.InstanceCount = 1;
        }

        public override IDxGeometry Convolute(IDxGeometry input)
        {
            IDxGeometry result = input.ShallowCopy();
            if (result is DX11IndexedGeometry)
            {
                DX11IndexedGeometry indexed = (DX11IndexedGeometry)result;
                DX11InstancedIndexedDrawer d = new DX11InstancedIndexedDrawer();
                d.InstanceCount = this.InstanceCount;
                indexed.AssignDrawer(d);
            }

            if (result is DX11VertexGeometry)
            {
                DX11VertexGeometry vertex = (DX11VertexGeometry)result;
                DX11InstancedVertexDrawer d = new DX11InstancedVertexDrawer();
                d.InstanceCount = this.InstanceCount;
                vertex.AssignDrawer(d);
            }

            return result;
        }
    }

    public class PerVertexDrawer : GeometryConvolution<DX11IndexedGeometry, DX11IndexedGeometry>
    {
        public PerVertexDrawer() { }

        public override DX11IndexedGeometry Convolute(DX11IndexedGeometry input)
        {
            DX11IndexedGeometry result = (DX11IndexedGeometry)input.ShallowCopy();
            DX11PerVertexIndexedDrawer drawer = new DX11PerVertexIndexedDrawer();
            result.AssignDrawer(drawer);
            return result;
        }
    }

    public class DefaultDrawer : GeometryConvolution<IDxGeometry, IDxGeometry>
    {
        public DefaultDrawer() { }

        public override IDxGeometry Convolute(IDxGeometry input)
        {
            IDxGeometry copy = input.ShallowCopy();
            if (copy is DX11IndexedGeometry)
            {
                DX11DefaultIndexedDrawer drawer = new DX11DefaultIndexedDrawer();
                ((DX11IndexedGeometry)copy).AssignDrawer(drawer);
            }
            else if (copy is DX11VertexGeometry)
            {
                DX11DefaultVertexDrawer drawer = new DX11DefaultVertexDrawer();
                ((DX11VertexGeometry)copy).AssignDrawer(drawer);
            }
            return copy;
        }
    }

    public class ChangeTopology : GeometryConvolution<IDxGeometry, IDxGeometry>
    {
        public PrimitiveTopology Topology { get; set; }

        public ChangeTopology()
        {
            this.Topology = PrimitiveTopology.Undefined;
        }

        public override IDxGeometry Convolute(IDxGeometry input)
        {
            IDxGeometry copy = input.ShallowCopy();
            if (this.Topology != PrimitiveTopology.Undefined)
            {
                copy.Topology = this.Topology;
            }
            return copy;
        }
    }

    public class SetBoundingBox : GeometryConvolution<IDxGeometry, IDxGeometry>
    {
        public Vector3 Minimum { get; set; }
        public Vector3 Maximum { get; set; }

        public SetBoundingBox()
        {
            this.Minimum = new Vector3(-1, -1, -1);
            this.Maximum = new Vector3(1, 1, 1);
        }

        public override IDxGeometry Convolute(IDxGeometry input)
        {
            IDxGeometry copy = input.ShallowCopy();
            BoundingBox b = new BoundingBox(this.Minimum, this.Maximum);
            copy.HasBoundingBox = true;
            copy.BoundingBox = b;
            return copy;
        }
    }


}
