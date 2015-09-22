using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

using Buffer = SharpDX.Direct3D11.Buffer;
using SharpDX;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;


namespace FeralTic.DX11.Resources
{
    public enum eRawBufferWriteMode { None, StreamOut, Uav }

    public struct RawBufferBindings
    {
        public bool AllowVertexBuffer;
        public bool AllowIndexBuffer;
        public eRawBufferWriteMode WriteMode;

        public RawBufferBindings(bool allowVbo, bool allowIbo, eRawBufferWriteMode writeMode)
        {
            this.AllowVertexBuffer = allowVbo;
            this.AllowIndexBuffer = allowIbo;
            this.WriteMode = writeMode;
        }
    }

    public unsafe class DX11RawBuffer : IDxBuffer, IDxShaderResource,IDxUnorderedResource, IDisposable
    {
        private DxDevice device;

        public Buffer Buffer { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public UnorderedAccessView UnorderedView { get; protected set; }
        public BufferDescription Description { get; protected set; }

        public int Size { get; protected set; }

        public static implicit operator Buffer(DX11RawBuffer buffer)
        {
            return buffer.Buffer;
        }

        protected DX11RawBuffer(DxDevice device, BufferDescription desc, IntPtr ptr, bool createUAV)
        {
            this.device = device;
            this.Size = desc.SizeInBytes;

            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, ptr, desc);
            this.Description = this.Buffer.Description;

            if (desc.BindFlags.HasFlag(BindFlags.ShaderResource))
            {
                ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
                {
                    Format = SharpDX.DXGI.Format.R32_Typeless,
                    Dimension = ShaderResourceViewDimension.ExtendedBuffer,

                    BufferEx = new ShaderResourceViewDescription.ExtendedBufferResource()
                    {
                        ElementCount = desc.SizeInBytes / 4,
                        FirstElement = 0,
                        Flags = ShaderResourceViewExtendedBufferFlags.Raw
                    }
                };

                this.ShaderView = new ShaderResourceView(device.Device, this.Buffer, srvd);
            }

            if (createUAV)
            {
                UnorderedAccessViewDescription uavd = new UnorderedAccessViewDescription()
                {
                    Format = SharpDX.DXGI.Format.R32_Typeless,
                    Dimension = UnorderedAccessViewDimension.Buffer,
                    Buffer = new UnorderedAccessViewDescription.BufferResource()
                    {
                        ElementCount = this.Size / 4,
                        FirstElement= 0,
                        Flags = UnorderedAccessViewBufferFlags.Raw
                    }
                };

                this.UnorderedView = new UnorderedAccessView(device.Device, this.Buffer, uavd);
            }
        }

        public static DX11RawBuffer CreateImmutable(DxDevice device, DataStream initial, RawBufferBindings binding)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferAllowRawViews,
                SizeInBytes = (int)initial.Length,
                Usage = ResourceUsage.Immutable
            };

            //Ignore UAV/StreamOut, since our buffer is immutable
            bd.BindFlags |= binding.AllowIndexBuffer ? BindFlags.IndexBuffer : 0;
            bd.BindFlags |= binding.AllowVertexBuffer ? BindFlags.VertexBuffer : 0;
            return new DX11RawBuffer(device, bd, initial.DataPointer, false);
        }

        public static DX11RawBuffer CreateDynamic(DxDevice device, int size)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.BufferAllowRawViews,
                SizeInBytes = size,
                Usage = ResourceUsage.Dynamic
            };

            return new DX11RawBuffer(device, bd, IntPtr.Zero, false);
        }

        public static DX11RawBuffer CreateWriteable(DxDevice device, int size, RawBufferBindings binding)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferAllowRawViews,
                SizeInBytes = size,
                Usage = ResourceUsage.Default,
                StructureByteStride = 4
            };

            bd.BindFlags |= binding.WriteMode == eRawBufferWriteMode.Uav ? BindFlags.UnorderedAccess : 0;
            bd.BindFlags |= binding.AllowIndexBuffer ? BindFlags.IndexBuffer : 0;
            bd.BindFlags |= binding.AllowVertexBuffer ? BindFlags.VertexBuffer : 0;
            bd.BindFlags |= binding.WriteMode == eRawBufferWriteMode.StreamOut ? BindFlags.StreamOutput : 0;


            return new DX11RawBuffer(device, bd, IntPtr.Zero, binding.WriteMode == eRawBufferWriteMode.Uav);
        }

        public static DX11RawBuffer CreateStaging(DX11RawBuffer buffer)
        {
            BufferDescription bd = buffer.Description;
            bd.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;
            bd.OptionFlags = ResourceOptionFlags.None;
            bd.Usage = ResourceUsage.Staging;
            bd.BindFlags = BindFlags.None;

            return new DX11RawBuffer(buffer.device, bd, IntPtr.Zero, false);
        }

        public static DX11RawBuffer CreateStaging(DxDevice device, IDxBuffer buffer)
        {
            BufferDescription bd = buffer.Buffer.Description;
            bd.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;
            bd.OptionFlags = ResourceOptionFlags.None;
            bd.Usage = ResourceUsage.Staging;
            bd.BindFlags = BindFlags.None;

            return new DX11RawBuffer(device, bd, IntPtr.Zero, false);
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

        public void Dispose()
        {
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }
}
