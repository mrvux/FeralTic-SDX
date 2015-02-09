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

    public unsafe class DX11VertexBuffer : IDxBuffer, IDisposable
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

        private DX11VertexBuffer(DxDevice device)
        {
            this.device = device;
        }

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

        public static DX11VertexBuffer CreateFromRawBuffer(DxDevice device, int verticesCount, int vertexSize, DX11RawBuffer buffer)
        {
            if (!buffer.Description.BindFlags.HasFlag(BindFlags.VertexBuffer))
            {
                throw new ArgumentException("Raw Buffer does not have Vertex Buffer Bind flag");
            }

            DX11VertexBuffer vbo = new DX11VertexBuffer(device);
            vbo.Buffer = buffer.Buffer;
            vbo.desc = buffer.Description;
            vbo.VertexSize = vertexSize;
            vbo.VerticesCount = verticesCount;

            return vbo;
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

        public static DX11VertexBuffer CreateStaging(DxDevice device, int elemenCount, int stride)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = stride * elemenCount,
                Usage = ResourceUsage.Staging
            };
            return new DX11VertexBuffer(device, elemenCount, stride, bd, null);
        }

        public static DX11VertexBuffer CreateStaging<T>(DxDevice device, int elemenCount) where T : struct
        {
            return CreateStaging(device, elemenCount, Marshal.SizeOf(typeof(T)));
        }

        public static DX11VertexBuffer CreateDynamic<T>(DxDevice device, int verticesCount, DataStream initial) where T : struct
        {
            return CreateDynamic(device, verticesCount, Marshal.SizeOf(typeof(T)), initial);
        }

        public DataStream MapForWrite(RenderContext context)
        {
            DataStream ds;
            context.Context.MapSubresource(this.Buffer, 0, MapMode.WriteDiscard, MapFlags.None, out ds);
            return ds;
        }

        public DataStream MapForRead(RenderContext context)
        {
            DataStream ds;
            context.Context.MapSubresource(this.Buffer, 0, MapMode.Read, MapFlags.None, out ds);
            return ds;
        }

        public void Unmap(RenderContext context)
        {
            context.Context.UnmapSubresource(this.Buffer, 0);
        }

        public void WriteData<T>(RenderContext context, T[] data) where T : struct
        {
            DataStream ds = this.MapForWrite(context);
            ds.WriteRange<T>(data);
            this.Unmap(context);
        }

        public T[] ReadData<T>(RenderContext context) where T : struct
        {
            DataStream ds = this.MapForRead(context);
            T[] result = ds.ReadRange<T>(this.VerticesCount);
            this.Unmap(context);
            return result;
        }

        public T[] ReadDataDebug<T>(RenderContext context) where T : struct
        {
            var staging = CreateStaging<T>(this.device, this.VerticesCount);
            context.Context.CopyResource(this.Buffer, staging.Buffer);
            T[] data = staging.ReadData<T>(context);
            staging.Dispose();
            return data;
        }

        public void Bind(RenderContext context, InputLayout layout, int slot = 0)
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
