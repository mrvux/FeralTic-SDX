using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11IndexedIndirectDrawer : IDX11GeometryDrawer<DX11IndexedGeometry>
    {
        private DX11IndexedGeometry geom;
        private IndexedIndirectBuffer indbuffer;

        public IndexedIndirectBuffer IndirectArgs { get { return this.indbuffer; } }

        public void Assign(DX11IndexedGeometry geometry)
        {
            this.geom = geometry;
        }

        public void Update(DX11RenderContext context, int defaultinstancecount)
        {
            if (this.indbuffer != null) { this.indbuffer.Dispose(); }

            DrawIndexedInstancedArgs args = new DrawIndexedInstancedArgs();
            args.BaseVertexLocation = 0;
            args.IndicesCount = this.geom.IndexBuffer.IndicesCount;
            args.InstanceCount = defaultinstancecount;
            args.StartIndexLocation = 0;
            args.StartInstanceLocation = 0;

            this.indbuffer = new IndexedIndirectBuffer(context.Device, args);

        }

        public void PrepareInputAssembler(DX11RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.InputLayout = layout;
            ctx.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer.Buffer, this.geom.VertexBuffer.VertexSize, 0));
            geom.IndexBuffer.Bind(ctx);
        }

        public void Draw(DX11RenderContext ctx)
        {
            ctx.Context.DrawIndexedInstancedIndirect(this.indbuffer.ArgumentBuffer, 0);
        }

        public void Dispose()
        {
            if (this.indbuffer != null) { this.indbuffer.Dispose(); }
        }
    }
}
