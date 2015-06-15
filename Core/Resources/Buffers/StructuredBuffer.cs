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
    public enum eDxBufferMode { Default = 0, Append = 2, Counter = 4 }

    public unsafe class DX11StructuredBuffer : IDxBuffer, IDxShaderResource,IDxUnorderedResource, IDisposable
    {
        private DxDevice device;

        public Buffer Buffer { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public UnorderedAccessView UnorderedView { get; protected set; }
        

        public int ElementCount { get; protected set; }
        public int Stride { get; protected set; }
        public eDxBufferMode BufferMode { get; protected set; }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public InputElement[] InputLayout { get; set; }

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

        public void WriteData<T>(RenderContext context, T[] data, int elementCount) where T : struct
        {
            DataStream ds = this.MapForWrite(context);
            ds.WriteRange<T>(data,0, elementCount);
            this.Unmap(context);
        }

        public T[] ReadDataDebug<T>(RenderContext context) where T : struct
        {
            var staging = CreateStaging<T>(this.device, this.ElementCount);
            context.Context.CopyResource(this.Buffer, staging.Buffer);
            T[] data = staging.ReadData<T>(context);
            staging.Dispose();
            return data;
        }

        public T[] ReadData<T>(RenderContext context) where T : struct
        {
            DataStream ds = this.MapForRead(context);
            T[] result = ds.ReadRange<T>(this.ElementCount);
            this.Unmap(context);
            return result;
        }

        public static DX11StructuredBuffer CreateTiled(DxDevice device, int elementCount, int stride, eDxBufferMode buffermode = eDxBufferMode.Default)
        {
            BufferDescription desc = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferStructured | ResourceOptionFlags.Tiled,
                SizeInBytes = elementCount * stride,
                StructureByteStride = stride,
                Usage = ResourceUsage.Default
            };
            return new DX11StructuredBuffer(device, elementCount, stride, desc, buffermode);
        }

        public static DX11StructuredBuffer CreateTiled<T>(DxDevice device, int elementCount, eDxBufferMode buffermode = eDxBufferMode.Default) where T : struct
        {
            int stride = Marshal.SizeOf(typeof(T));
            return CreateTiled(device, elementCount, stride, buffermode);
        }

        private DX11StructuredBuffer()
        {

        }

        protected DX11StructuredBuffer(DxDevice device, int elementcount, int stride, BufferDescription desc, DataStream initial = null)
        {
            this.device = device;
            this.ElementCount = elementcount;
            this.Stride = stride;
            this.BufferMode = eDxBufferMode.Default;

            if (initial != null)
            {
                this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, initial, desc);
            }
            else
            {
                this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, desc);
            }

            if (desc.Usage != ResourceUsage.Staging)
            {
                this.ShaderView = new ShaderResourceView(device.Device, this.Buffer);
            }
        }

        protected DX11StructuredBuffer(DxDevice device, int elementcount, int stride, BufferDescription desc, IntPtr ptr)
        {
            this.device = device;
            this.ElementCount = elementcount;
            this.Stride = stride;
            this.BufferMode = eDxBufferMode.Default;
            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device,ptr, desc);
            this.ShaderView = new ShaderResourceView(device.Device, this.Buffer);
        }

        protected DX11StructuredBuffer(DxDevice device, int elementcount, int stride, BufferDescription desc, eDxBufferMode buffermode = eDxBufferMode.Default)
        {
            this.device = device;
            this.ElementCount = elementcount;
            this.Stride = stride;
            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, desc);
            this.ShaderView = new ShaderResourceView(device.Device, this.Buffer);
            this.BufferMode = buffermode;

            UnorderedAccessViewDescription uavd = new UnorderedAccessViewDescription()
            {
                Format = SharpDX.DXGI.Format.Unknown,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Buffer = new UnorderedAccessViewDescription.BufferResource()
                {
                    ElementCount = this.ElementCount,
                    Flags = (UnorderedAccessViewBufferFlags)buffermode
                }
            };

            this.UnorderedView = new UnorderedAccessView(device.Device, this.Buffer,uavd);
        }

        public static DX11StructuredBuffer CreateImmutable(DxDevice device, int elemenCount, int stride, DataStream initial)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferStructured,
                SizeInBytes = stride * elemenCount,
                StructureByteStride = stride,
                Usage = ResourceUsage.Immutable
            };
            return new DX11StructuredBuffer(device, elemenCount, stride, bd, initial);
        }

        public static DX11StructuredBuffer CreateImmutable<T>(DxDevice device, T[] initial) where T : struct
        {
            int stride = Marshal.SizeOf(typeof(T));
            DataStream ds = new DataStream(stride * initial.Length, true, true);
            ds.WriteRange<T>(initial);
            ds.Position = 0;

            DX11StructuredBuffer sb = CreateImmutable(device, initial.Length, stride, ds);
            ds.Dispose();
            return sb;
        }

        public static DX11StructuredBuffer CreateDynamic(DxDevice device, int elemenCount, int stride)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.BufferStructured,
                SizeInBytes = stride * elemenCount,
                StructureByteStride = stride,
                Usage = ResourceUsage.Dynamic
            };
            return new DX11StructuredBuffer(device, elemenCount, stride, bd, null);
        }

        public static DX11StructuredBuffer CreateDynamic<T>(DxDevice device, int elemenCount) where T : struct
        {
            return CreateDynamic(device, elemenCount, Marshal.SizeOf(typeof(T)));
        }

        public static DX11StructuredBuffer CreateStaging(DxDevice device, int elemenCount, int stride)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.BufferStructured,
                SizeInBytes = stride * elemenCount,
                StructureByteStride = stride,
                Usage = ResourceUsage.Staging
            };
            return new DX11StructuredBuffer(device, elemenCount, stride, bd, null);
        }

        public DX11StructuredBuffer AsStaging()
        {
            BufferDescription bd = this.Buffer.Description;
            bd.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;
            bd.OptionFlags = ResourceOptionFlags.None;
            bd.Usage = ResourceUsage.Staging;
            bd.BindFlags = BindFlags.None;

            DX11StructuredBuffer result = new DX11StructuredBuffer(this.device, this.ElementCount, this.Stride, bd, null);
            return result;
        }

        public static DX11StructuredBuffer CreateStaging<T>(DxDevice device, int elemenCount) where T : struct
        {
            return CreateStaging(device, elemenCount, Marshal.SizeOf(typeof(T)));
        }

        public static DX11StructuredBuffer CreateWriteable(DxDevice device, int elemenCount, int stride, eDxBufferMode mode = eDxBufferMode.Default)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferStructured,
                SizeInBytes = stride * elemenCount,
                StructureByteStride = stride,
                Usage = ResourceUsage.Default
            };
            return new DX11StructuredBuffer(device, elemenCount, stride, bd, mode);
        }

        public static DX11StructuredBuffer CreateWriteable<T>(DxDevice device, int elementcount, eDxBufferMode mode = eDxBufferMode.Default) where T : struct
        {
            return CreateWriteable(device, elementcount, Marshal.SizeOf(typeof(T)), mode);
        }

        public static DX11StructuredBuffer CreateAppend<T>(DxDevice device, int elementcount) where T : struct
        {
            return CreateWriteable(device, elementcount, Marshal.SizeOf(typeof(T)), eDxBufferMode.Append);
        }

        public static DX11StructuredBuffer CreateCounter<T>(DxDevice device, int elementcount) where T : struct
        {
            return CreateWriteable(device, elementcount, Marshal.SizeOf(typeof(T)), eDxBufferMode.Counter);
        }

        public void Dispose()
        {
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }
}
