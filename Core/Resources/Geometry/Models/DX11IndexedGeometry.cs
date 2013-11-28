using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Buffer = SharpDX.Direct3D11.Buffer;

using SharpDX;
using SharpDX.Direct3D11;


//using FeralTic.Resources.Geometry;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Resources
{
    public class DX11IndexedGeometry : DX11BaseGeometry
    {
        private IDX11GeometryDrawer<DX11IndexedGeometry> drawer;

        public IDX11GeometryDrawer<DX11IndexedGeometry> Drawer { get { return this.drawer; } }

        public DX11VertexBuffer VertexBuffer { get; set; }
        public DX11IndexBuffer IndexBuffer { get; set; }

        private bool ownsvbo;
        private bool ownsido;

        public DX11IndexedGeometry(DxDevice device)
            : base(device)
        {
            this.drawer = new DX11DefaultIndexedDrawer();
            this.drawer.Assign(this);

            this.ownsvbo = true;
            this.ownsido = true;
        }

        public void AssignDrawer(IDX11GeometryDrawer<DX11IndexedGeometry> drawer)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
        }

        public override void Draw(DX11RenderContext ctx)
        {
            this.drawer.Draw(ctx);
        }

        public DX11IndexedGeometry(DX11IndexedGeometry owner)
        {
            this.ownsvbo = false;
            this.ownsido = false;

            this.device = owner.device;
            this.drawer = owner.drawer;
            this.IndexBuffer = owner.IndexBuffer;
            this.InputLayout = owner.InputLayout;
            this.Topology = owner.Topology;
            this.VertexBuffer = owner.VertexBuffer;
        }

        public static DX11IndexedGeometry CreateFrom<T>(DxDevice device, T[] vertices, int[] indices, InputElement[] layout) where T : struct
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable<T>(device, vertices) ;
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, indices);
            geom.InputLayout = layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.HasBoundingBox = false;
            return geom;
        }

        public override void Bind(DX11RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.PrimitiveTopology = this.Topology;
            drawer.PrepareInputAssembler(ctx, layout);
        }

        public override void Dispose()
        {
            if (this.ownsvbo) { if (this.VertexBuffer != null) { this.VertexBuffer.Dispose(); } }
            if (this.ownsido) { if (this.IndexBuffer != null) { this.IndexBuffer.Dispose(); } }
        }

        public override IDX11Geometry ShallowCopy()
        {
            return new DX11IndexedGeometry(this);
        }
    }
}
