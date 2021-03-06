﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Resources
{
    public class DX11VertexIndirectDrawer : IDX11GeometryDrawer<DX11VertexGeometry>
    {
        private DX11VertexGeometry geom;
        private InstancedIndirectBuffer indbuffer;

        public InstancedIndirectBuffer IndirectArgs { get { return this.indbuffer; } }

        public void Assign(DX11VertexGeometry geometry)
        {
            this.geom = geometry;
        }

        public void Update(RenderContext context, int defaultinstancecount)
        {
            if (this.indbuffer != null) { this.indbuffer.Dispose(); }

            DrawInstancedArgs args = new DrawInstancedArgs();
            args.InstanceCount = defaultinstancecount;
            args.StartInstanceLocation = 0;
            args.StartVertexLocation = 0;
            args.VertexCount = this.geom.VerticesCount;

            this.indbuffer = new InstancedIndirectBuffer(context.Device, args);
        }

        public void CopyInstanceCount(DeviceContext ctx, Buffer buffer, int offset)
        {
            ResourceRegion region = new ResourceRegion(offset, 0, 0, offset + 4, 1, 1);
            ctx.CopySubresourceRegion(buffer, 0, region, this.indbuffer.ArgumentBuffer, 0, 4, 0, 0);
        }

        public void PrepareInputAssembler(RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.InputLayout = layout;
            ctx.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer, this.geom.VertexSize, 0));
        }

        public void Draw(RenderContext ctx)
        {
            ctx.Context.DrawInstancedIndirect(this.indbuffer.ArgumentBuffer, 0);
        }

        public void Dispose()
        {
            if (this.indbuffer != null) { this.indbuffer.Dispose(); }
        }
    }
}
