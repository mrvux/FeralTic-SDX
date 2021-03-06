﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Auto drawer for vertex geometry
    /// </summary>
    public class DX11VertexAutoDrawer : IDX11GeometryDrawer<DX11VertexGeometry>
    {
        protected DX11VertexGeometry geom;

        public void Assign(DX11VertexGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.InputLayout = layout;
            ctx.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer, this.geom.VertexSize, 0));
        }

        public virtual void Draw(RenderContext ctx)
        {
            ctx.Context.DrawAuto();
        }
    }
}
