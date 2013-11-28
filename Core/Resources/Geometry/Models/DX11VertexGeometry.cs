using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

using Buffer = SharpDX.Direct3D11.Buffer;


namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Geometry with a single vertex buffer
    /// </summary>
    public class DX11VertexGeometry : DX11BaseGeometry
    {    
        private IDX11GeometryDrawer<DX11VertexGeometry> drawer;
        private bool ownsvbo;



        public IDX11GeometryDrawer<DX11VertexGeometry> Drawer { get { return this.drawer; } }

        public DX11VertexGeometry(DxDevice device)
            : base(device)
        {
            this.drawer = new DX11DefaultVertexDrawer();
            this.drawer.Assign(this);
            this.ownsvbo = true;
        }

        public DX11VertexGeometry(DX11VertexGeometry owner)
        {
            this.ownsvbo = false;

            this.device = owner.device;
            this.drawer = owner.drawer;
            this.InputLayout = owner.InputLayout;
            this.Topology = owner.Topology;
            this.VertexBuffer = owner.VertexBuffer;
            this.VertexSize = owner.VertexSize;
            this.VerticesCount = owner.VerticesCount; 
        }


        public void AssignDrawer(IDX11GeometryDrawer<DX11VertexGeometry> drawer)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
        }

        public Buffer VertexBuffer { get; set; }
        public int VerticesCount { get; set; }
        public int VertexSize { get; set; }

        public override void Bind(RenderContext context,InputLayout layout)
        {
            context.Context.InputAssembler.PrimitiveTopology = this.Topology;
            this.drawer.PrepareInputAssembler(context, layout);
        }

        public override void Draw(RenderContext context)
        {
            this.drawer.Draw(context);
        }

        public override void Dispose()
        {
            if (this.ownsvbo)
            {
                if (this.VertexBuffer != null) { this.VertexBuffer.Dispose(); }
            }
        }

        public override IDX11Geometry ShallowCopy()
        {
            return new DX11VertexGeometry(this);
        }
    }
}
