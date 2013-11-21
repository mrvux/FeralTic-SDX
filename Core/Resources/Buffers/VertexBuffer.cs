using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

using Buffer = SharpDX.Direct3D11.Buffer;
using SharpDX;
using System.Runtime.InteropServices;


namespace FeralTic.DX11.Resources
{
    public unsafe class DX11VertexBuffer : IDX11Resource, IDisposable
    {
        private DX11Device device;

        public Buffer Buffer { get; protected set; }
        public int VerticesCount { get; protected set; }
        public int VertexSize { get; protected set; }

        private BufferDescription desc;

        public bool AllowStreamOutput
        {
            get { return this.desc.BindFlags.HasFlag(BindFlags.StreamOutput); }
        }


        public bool AllowRaw
        {
            get { return this.desc.OptionFlags.HasFlag(ResourceOptionFlags.BufferAllowRawViews); }
        }


        public int TotalSize
        {
            get { return this.VertexSize * this.VerticesCount; }
        }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public InputElement[] InputLayout { get; set; }

        protected DX11VertexBuffer(DX11Device device, int verticescount, int vertexsize, BufferDescription desc, DataStream initial = null)
        {
            this.device = device;
            this.VertexSize = vertexsize;
            this.VerticesCount = verticescount;
            
            if (initial != null)
            {
                initial.Position = 0;
                this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, initial, desc);
            }
            else
            {
                this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, desc);
            }

            this.desc = this.Buffer.Description;
        }

        protected DX11VertexBuffer(DX11Device device, int verticescount, int vertexsize, BufferDescription desc, IntPtr ptr)
        {
            this.device = device;
            this.VertexSize = vertexsize;
            this.VerticesCount = verticescount;

            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, ptr, desc);
            this.desc = this.Buffer.Description;
        }

        public static DX11VertexBuffer CreateWriteable(DX11Device device, int verticesCount, int vertexSize, bool streamout, bool raw)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = vertexSize * verticesCount,
                Usage = ResourceUsage.Default
            };

            if (streamout) { bd.BindFlags |= BindFlags.StreamOutput; }

            if (raw) 
            { 
                bd.BindFlags |= BindFlags.ShaderResource; 
                bd.BindFlags |= BindFlags.UnorderedAccess;
                bd.OptionFlags = ResourceOptionFlags.BufferAllowRawViews;
            }

            return new DX11VertexBuffer(device, verticesCount, vertexSize, bd, null);
        }

        public static DX11VertexBuffer CreateImmutable(DX11Device device, int verticesCount, int vertexSize, DataStream initial)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = vertexSize * verticesCount,
                Usage = ResourceUsage.Immutable
            };

            return new DX11VertexBuffer(device, verticesCount, vertexSize, bd, initial);
        }

        public static DX11VertexBuffer CreateImmutable<T>(DX11Device device, T[] initial) where T : struct
        {
            int vertexSize = Marshal.SizeOf(typeof(T));
            int verticesCount = initial.Length;

            DataStream ds = new DataStream(vertexSize * verticesCount,true,true);
            ds.WriteRange<T>(initial);
            ds.Position = 0;
            DX11VertexBuffer result = CreateImmutable(device, verticesCount, vertexSize, ds);
            ds.Dispose();
            return result;  
        }

        public static DX11VertexBuffer CreateImmutable<T>(DX11Device device, DataStream initial) where T : struct
        {
            int vertexSize = Marshal.SizeOf(typeof(T));
            int verticesCount = (int)initial.Length / vertexSize;
            DX11VertexBuffer result = CreateImmutable(device, verticesCount, vertexSize, initial);
            return result;
        }

        public static DX11VertexBuffer CreateDynamic(DX11Device device, int verticesCount, int vertexSize, DataStream initial)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = vertexSize * verticesCount,
                Usage = ResourceUsage.Dynamic
            };

            return new DX11VertexBuffer(device, verticesCount, vertexSize, bd, initial);
        }

        public void Bind(DX11RenderContext context, InputLayout layout, int slot = 0)
        {
            context.Context.InputAssembler.InputLayout = layout;
            context.Context.InputAssembler.SetVertexBuffers(slot, new VertexBufferBinding(this.Buffer, this.VertexSize, 0));
        }

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }
}
