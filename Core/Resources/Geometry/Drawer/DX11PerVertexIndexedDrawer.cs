using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11PerVertexIndexedDrawer : IDX11GeometryDrawer<DX11IndexedGeometry>
    {
        protected DX11IndexedGeometry geom;

        public void Assign(DX11IndexedGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(DX11RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.InputLayout = layout;
            ctx.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer.Buffer, this.geom.VertexBuffer.VertexSize, 0));
        }

        public virtual void Draw(DX11RenderContext ctx)
        {
            ctx.Context.Draw(this.geom.VertexBuffer.VerticesCount, 0);
        }
    }
}
