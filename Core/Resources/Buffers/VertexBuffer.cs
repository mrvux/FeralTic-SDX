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
    public enum eVertexBufferWriteMode { None, Raw, StreamOut }

    public unsafe class DX11VertexBuffer : IDX11Resource, IDisposable
    {
        private DxDevice device;

        public Buffer Buffer { get; protected set; }
        public int VerticesCount { get; protected set; }
        public int VertexSize { get; protected set; }

        private BufferDescription desc;

        public bool AllowStreamOutput
        {
            get { return this.desc.BindFlags.HasFlag(BindFlags.StreamOutput); }
        }


        public bool AllowUAV
        {
            get { return this.desc.BindFlags.HasFlag(BindFlags.UnorderedAccess); }
        }

        public eVertexBufferWriteMode WriteMode
        {
            get
            {
                if (this.AllowStreamOutput) { return eVertexBufferWriteMode.StreamOut; }
                else if (this.AllowUAV) { return eVertexBufferWriteMode.Raw; }
                else { return eVertexBufferWriteMode.None; }
            }
        }


        public int TotalSize
        {
            get { return this.VertexSize * this.VerticesCount; }
        }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public InputElement[] InputLayout { get; set; }

        protected DX11VertexBuffer(DxDevice device, int verticescount, int vertexsize, BufferDescription desc, DataStream initial = null)
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

        protected DX11VertexBuffer(DxDevice device, int verticescount, int vertexsize, BufferDescription desc, IntPtr ptr)
        {
            this.device = device;
            this.VertexSize = vertexsize;
            this.VerticesCount = verticescount;

            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, ptr, desc);
            this.desc = this.Buffer.Description;
        }

        public static DX11VertexBuffer CreateWriteable(DxDevice device, int verticesCount, int vertexSize, eVertexBufferWriteMode mode = eVertexBufferWriteMode.None)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = vertexSize * verticesCount,
                Usage = ResourceUsage.Default
            };

            bd.BindFlags |= BindFlags.ShaderResource;
            bd.OptionFlags = ResourceOptionFlags.BufferAllowRawViews;

            if (mode == eVertexBufferWriteMode.StreamOut)
            {
                bd.BindFlags |= BindFlags.StreamOutput;
            }
            else if (mode == eVertexBufferWriteMode.Raw)
            {
                bd.BindFlags |= BindFlags.UnorderedAccess;
            }
            return new DX11VertexBuffer(device, verticesCount, vertexSize, bd, null);
        }

        public static DX11VertexBuffer CreateImmutable(DxDevice device, int verticesCount, int vertexSize, DataStream initial)
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

        public static DX11VertexBuffer CreateImmutable<T>(DxDevice device, T[] initial) where T : struct
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

        public static DX11VertexBuffer CreateImmutable<T>(DxDevice device, DataStream initial) where T : struct
        {
            int vertexSize = Marshal.SizeOf(typeof(T));
            int verticesCount = (int)initial.Length / vertexSize;
            DX11VertexBuffer result = CreateImmutable(device, verticesCount, vertexSize, initial);
            return result;
        }

        public static DX11VertexBuffer CreateDynamic(DxDevice device, int verticesCount, int vertexSize, DataStream initial)
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
