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
    public enum eDX11BufferMode { Default = 0, Append = 2, Counter = 4 }

    public unsafe class DX11StructuredBuffer : IDX11ShaderResource,IDX11UnorderedResource, IDisposable
    {
        private DX11Device device;

        public Buffer Buffer { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public UnorderedAccessView UnorderedView { get; protected set; }

        public int ElementCount { get; protected set; }
        public int Stride { get; protected set; }
        public eDX11BufferMode BufferMode { get; protected set; }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public InputElement[] InputLayout { get; set; }

        public DataStream MapForWrite(DX11RenderContext context)
        {
            DataStream ds;
            context.Context.MapSubresource(this.Buffer, 0, MapMode.WriteDiscard, MapFlags.None, out ds);
            return ds;
        }

        public DataStream MapForRead(DX11RenderContext context)
        {
            DataStream ds;
            context.Context.MapSubresource(this.Buffer, 0, MapMode.Read, MapFlags.None, out ds);
            return ds;
        }

        public void Unmap(DX11RenderContext context)
        {
            context.Context.UnmapSubresource(this.Buffer, 0);
        }

        public void WriteData<T>(DX11RenderContext context, T[] data) where T : struct
        {
            DataStream ds = this.MapForWrite(context);
            ds.WriteRange<T>(data);
            this.Unmap(context);
        }

        public T[] ReadData<T>(DX11RenderContext context) where T : struct
        {
            DataStream ds = this.MapForRead(context);
            T[] result = ds.ReadRange<T>(this.ElementCount);
            this.Unmap(context);
            return result;
        }

        protected DX11StructuredBuffer(DX11Device device, int elementcount, int stride, BufferDescription desc, DataStream initial = null)
        {
            this.device = device;
            this.ElementCount = elementcount;
            this.Stride = stride;
            this.BufferMode = eDX11BufferMode.Default;

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

        protected DX11StructuredBuffer(DX11Device device, int elementcount, int stride, BufferDescription desc, IntPtr ptr)
        {
            this.device = device;
            this.ElementCount = elementcount;
            this.Stride = stride;
            this.BufferMode = eDX11BufferMode.Default;
            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device,ptr, desc);
            this.ShaderView = new ShaderResourceView(device.Device, this.Buffer);
        }

        protected DX11StructuredBuffer(DX11Device device, int elementcount, int stride, BufferDescription desc, eDX11BufferMode buffermode = eDX11BufferMode.Default)
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

        public static DX11StructuredBuffer CreateImmutable(DX11Device device, int elemenCount, int stride, DataStream initial)
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

        public static DX11StructuredBuffer CreateImmutable<T>(DX11Device device, T[] initial) where T : struct
        {
            int stride = Marshal.SizeOf(typeof(T));
            DataStream ds = new DataStream(stride * initial.Length, true, true);
            ds.WriteRange<T>(initial);
            ds.Position = 0;

            DX11StructuredBuffer sb = CreateImmutable(device, initial.Length, stride, ds);
            ds.Dispose();
            return sb;
        }

        public static DX11StructuredBuffer CreateDynamic(DX11Device device, int elemenCount, int stride)
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

        public static DX11StructuredBuffer CreateDynamic<T>(DX11Device device, int elemenCount) where T : struct
        {
            return CreateDynamic(device, elemenCount, Marshal.SizeOf(typeof(T)));
        }

        public static DX11StructuredBuffer CreateStaging(DX11Device device, int elemenCount, int stride)
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

        public static DX11StructuredBuffer CreateStaging<T>(DX11Device device, int elemenCount) where T : struct
        {
            return CreateStaging(device, elemenCount, Marshal.SizeOf(typeof(T)));
        }

        public static DX11StructuredBuffer CreateWriteable(DX11Device device, int elemenCount, int stride, eDX11BufferMode mode = eDX11BufferMode.Default)
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

        public static DX11StructuredBuffer CreateWriteable<T>(DX11Device device, int elementcount, eDX11BufferMode mode = eDX11BufferMode.Default) where T : struct
        {
            return CreateWriteable(device, elementcount, Marshal.SizeOf(typeof(T)), mode);
        }

        public static DX11StructuredBuffer CreateAppend<T>(DX11Device device, int elementcount) where T : struct
        {
            return CreateWriteable(device, elementcount, Marshal.SizeOf(typeof(T)), eDX11BufferMode.Append);
        }

        public static DX11StructuredBuffer CreateCounter<T>(DX11Device device, int elementcount) where T : struct
        {
            return CreateWriteable(device, elementcount, Marshal.SizeOf(typeof(T)), eDX11BufferMode.Counter);
        }

        public void Dispose()
        {
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }
}
