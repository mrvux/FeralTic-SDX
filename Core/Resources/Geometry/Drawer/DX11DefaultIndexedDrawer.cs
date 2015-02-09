using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11DefaultIndexedDrawer : IDX11GeometryDrawer<DX11IndexedGeometry>
    {
        protected DX11IndexedGeometry geom;

        public void Assign(DX11IndexedGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.InputLayout = layout;
            ctx.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer.Buffer, this.geom.VertexBuffer.VertexSize, 0));
            geom.IndexBuffer.Bind(ctx);
        }

        public virtual void Draw(RenderContext ctx)
        {
            ctx.Context.DrawIndexed(this.geom.IndexBuffer.IndicesCount, 0, 0);
        }
    }

    public class DX11CustomIndexedDrawer : IDX11GeometryDrawer<DX11IndexedGeometry>
    {
        protected DX11IndexedGeometry geom;

        public void Assign(DX11IndexedGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.InputLayout = layout;
            ctx.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer.Buffer, this.geom.VertexBuffer.VertexSize, 0));
            geom.IndexBuffer.Bind(ctx);
        }

        public int IndicesCount { get; set; }
        public int StartIndex { get; set; }
 
        public virtual void Draw(RenderContext ctx)
        {
            ctx.Context.DrawIndexed(this.IndicesCount, this.StartIndex, 0);
        }
    }

    /*public class DX11DefaultIndexOnlyDrawer : IDX11GeometryDrawer<DX11IndexOnlyGeometry>
    {
        protected DX11IndexOnlyGeometry geom;

        public void Assign(DX11IndexOnlyGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {
            VertexBufferBinding vb = new VertexBufferBinding();
            ctx.InputAssembler.SetVertexBuffers(0, vb);
            geom.IndexBuffer.Bind();
        }

        public virtual void Draw(DeviceContext ctx)
        {
            ctx.DrawIndexed(this.geom.IndexBuffer.IndicesCount, 0, 0);
        }
    }*/
}
