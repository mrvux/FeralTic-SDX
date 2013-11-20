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
    public unsafe class DX11RawBuffer : IDX11ShaderResource,IDX11UnorderedResource, IDisposable
    {
        private DX11Device device;

        public Buffer Buffer { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public UnorderedAccessView UnorderedView { get; protected set; }
        public BufferDescription Description { get; protected set; }

        public int Size { get; protected set; }

        protected DX11RawBuffer(DX11Device device, BufferDescription desc, IntPtr ptr, bool createUAV)
        {
            this.device = device;
            this.Size = desc.SizeInBytes;

            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, ptr, desc);
            this.Description = this.Buffer.Description;

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

            this.ShaderView = new ShaderResourceView(device.Device, this.Buffer,srvd);

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

        public static DX11RawBuffer CreateImmutable(DX11Device device, DataStream initial)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferAllowRawViews,
                SizeInBytes = (int)initial.Length,
                Usage = ResourceUsage.Immutable
            };
            return new DX11RawBuffer(device, bd, initial.DataPointer, false);
        }

        public static DX11RawBuffer CreateWriteable(DX11Device device, int size)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferAllowRawViews,
                SizeInBytes = size,
                Usage = ResourceUsage.Default,
                StructureByteStride = 4
            };
            return new DX11RawBuffer(device, bd, IntPtr.Zero, true);
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

        public void Dispose()
        {
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }
}
