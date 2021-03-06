﻿using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Resources
{
    public class DX11Geometry : DX11BaseGeometry
    {
        public DX11IndexBuffer IndexBuffer { get; protected set; }

        public bool HasIndexBuffer { get; protected set; }

        private List<DX11VertexBuffer> vertexBuffers;

        private VertexBufferBinding[] binding;

        public DX11Geometry()
        {
            this.binding = new VertexBufferBinding[0];
            this.vertexBuffers = new List<DX11VertexBuffer>();
        }

        public void AddVertexBuffer(DX11VertexBuffer buffer)
        {
            this.vertexBuffers.Add(buffer);
        }

        private void UpdateVertexLayouts()
        {
            List<VertexBufferBinding> newbinding = new List<VertexBufferBinding>();
            List<InputElement> newlayout = new List<InputElement>();

            for (int i = 0; i < vertexBuffers.Count; i++)
            {
                DX11VertexBuffer vb = vertexBuffers[i];
                VertexBufferBinding vbind = new VertexBufferBinding(vb.Buffer, vb.VertexSize, 0);
                newbinding.Add(vbind);

                foreach (InputElement elem in vb.InputLayout)
                {
                    InputElement newelem = elem;
                    newelem.Slot = i;
                    newlayout.Add(newelem);
                }
            }

            this.InputLayout = newlayout.ToArray();
            this.binding = newbinding.ToArray();
        }

        public override void Draw(RenderContext context)
        {
            if (this.HasIndexBuffer)
            {
                context.Context.DrawIndexed(this.IndexBuffer.IndicesCount, 0, 0);
            }
            else
            {
                if (this.vertexBuffers.Count == 0)
                {
                    throw new InvalidOperationException("Standard geometry must have either an indexbuffer or at least one vertexbuffer");
                }
                else
                {
                    context.Context.Draw(this.vertexBuffers[0].VerticesCount, 0);
                }
            }
        }

        public override void Bind(RenderContext context, InputLayout layout)
        {
            context.Context.InputAssembler.SetVertexBuffers(0, this.binding);
            context.Context.InputAssembler.InputLayout = layout;
            if (this.HasIndexBuffer)
            {
                this.IndexBuffer.Bind(context);
            }
        }

        public override void Dispose()
        {
            foreach (DX11VertexBuffer vbo in vertexBuffers) { vbo.Dispose(); }
            if (this.IndexBuffer != null) { this.IndexBuffer.Dispose(); this.IndexBuffer = null; }
        }

        public override IDxGeometry ShallowCopy()
        {
            return null;
        }
    }
}
