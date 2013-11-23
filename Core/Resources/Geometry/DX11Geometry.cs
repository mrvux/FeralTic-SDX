using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Resources
{
    public class DX11Geometry
    {
        public PrimitiveTopology Topology { get; set; }

        public DX11IndexBuffer IndexBuffer { get; protected set; }

        public InputElement[] InputLayout { get; protected set; }

        private VertexBufferBinding[] binding = new VertexBufferBinding[0];
        private List<DX11VertexBuffer> vertexbuffers = new List<DX11VertexBuffer>();

        public void AddVertexBuffer(DX11VertexBuffer vbo)
        {
            this.vertexbuffers.Add(vbo);
            this.RebuildLayout();
        }

        private void RebuildLayout()
        {
            List<InputElement> elements = new List<InputElement>();
            List<VertexBufferBinding> bindings = new List<VertexBufferBinding>();

            for (int i = 0; i < this.vertexbuffers.Count; i++)
            {
                DX11VertexBuffer vbo = this.vertexbuffers[i];
                bindings.Add(new VertexBufferBinding(vbo.Buffer, vbo.VertexSize, 0));

                for (int j = 0; j < vbo.InputLayout.Length; j++)
                {
                    InputElement elem = vbo.InputLayout[j];
                    elem.Slot = i;
                    elements.Add(elem);
                }
            }
            
            this.InputLayout = elements.ToArray();
            this.binding = bindings.ToArray();
        }

        public void Bind(DX11RenderContext context, InputLayout layout, PrimitiveTopology topology = PrimitiveTopology.Undefined)
        {
            DeviceContext2 ctx = context;

            ctx.InputAssembler.InputLayout = layout;
            ctx.InputAssembler.PrimitiveTopology = topology == PrimitiveTopology.Undefined ? this.Topology : topology;

            if (this.IndexBuffer != null)
            {
                this.IndexBuffer.Bind(context);
            }

            ctx.InputAssembler.SetVertexBuffers(0, binding);
        }

        public void Draw(DX11RenderContext context)
        {
            if (this.IndexBuffer == null && this.vertexbuffers.Count == 0)
            {
                return;
            }

            if (this.IndexBuffer != null)
            {

            }
        }
    }
}
