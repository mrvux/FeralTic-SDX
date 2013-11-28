using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SharpDX.Direct3D11;
using SharpDX;
using Buffer = SharpDX.Direct3D11.Buffer;


namespace FeralTic.DX11.Resources
{
    public class BaseIndirectBuffer<T> where T : struct
    {
        private DxDevice device;
        private Buffer staging;

        public Buffer ArgumentBuffer { get; protected set; }

        public DX11StructuredBuffer WriteBuffer { get; protected set; }

        public BaseIndirectBuffer(DxDevice device, T args)
        {
            this.device = device;

            int size = Marshal.SizeOf(args);

            BufferDescription bd = new BufferDescription();
            bd.Usage = ResourceUsage.Default;
            bd.StructureByteStride = 0;
            bd.SizeInBytes = size;
            bd.CpuAccessFlags = CpuAccessFlags.None;
            bd.BindFlags = BindFlags.None;
            bd.OptionFlags = ResourceOptionFlags.DrawIndirectArguments;

            DataStream dsb = new DataStream(size, false, true);
            dsb.Position = 0;
            dsb.Write<T>(args);
            dsb.Position = 0;

            this.ArgumentBuffer = new SharpDX.Direct3D11.Buffer(device.Device, dsb, bd);
            dsb.Dispose();

            this.WriteBuffer = DX11StructuredBuffer.CreateWriteable(device, size / 4, 4);
        }

        public void UpdateArgumentBuffer(DX11RenderContext context)
        {
            context.Context.CopyResource(this.WriteBuffer.Buffer, this.ArgumentBuffer);
        }
        
        public void Dispose()
        {
            this.ArgumentBuffer.Dispose();
            this.WriteBuffer.Dispose();

            if (this.staging != null) { this.staging.Dispose(); }
        }

        public T RetrieveArgs(DX11RenderContext context)
        {

            if (staging == null)
            {
                int size = Marshal.SizeOf(typeof(T));

                BufferDescription bd = new BufferDescription();
                bd.Usage = ResourceUsage.Staging;
                bd.StructureByteStride = 0;
                bd.SizeInBytes = size;
                bd.CpuAccessFlags = CpuAccessFlags.Read;
                bd.BindFlags = BindFlags.None;
                bd.OptionFlags = ResourceOptionFlags.None;

                staging = new SharpDX.Direct3D11.Buffer(device.Device, bd);
            }

            context.Context.CopyResource(this.ArgumentBuffer, staging);

            DataStream ds;
            context.Context.MapSubresource(staging, MapMode.Read, MapFlags.None,out ds);
            T data = ds.Read<T>();
            context.Context.UnmapSubresource(staging, 0);

            return data;
        }
    }
}
