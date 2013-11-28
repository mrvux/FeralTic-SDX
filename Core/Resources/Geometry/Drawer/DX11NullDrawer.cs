using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11NullDrawer : IDX11GeometryDrawer<DX11NullGeometry>
    {
        private DX11NullGeometry geom;

        public int VertexCount { get; set; }

        public DX11NullDrawer()
        {
            this.VertexCount = 1;
        }
        
        public void Assign(DX11NullGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(DX11RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.InputLayout = null;
            ctx.Context.InputAssembler.SetIndexBuffer(null, SharpDX.DXGI.Format.Unknown, 0);
            VertexBufferBinding vb = new VertexBufferBinding();
            ctx.Context.InputAssembler.SetVertexBuffers(0, vb);
        }

        public virtual void Draw(DX11RenderContext ctx)
        {
            ctx.Context.Draw(this.VertexCount,0);
        }
    }
}
