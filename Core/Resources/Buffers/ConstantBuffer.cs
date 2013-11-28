using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SharpDX;
using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Generic implementation for constant buffer
    /// </summary>
    /// <typeparam name="T">Buffer structure type</typeparam>
    public class DX11ConstantBuffer<T> where T :struct
    {
        private DxDevice device;

        public DX11ConstantBuffer(DxDevice device)
            : this(device, false)
        {
        }

        public DX11ConstantBuffer(DxDevice device, bool align)
        {
            this.device = device;
            int size;
            if (align)
            {
                size = ((Marshal.SizeOf(typeof(T)) + 15) / 16) * 16;
            }
            else
            {
                size = Marshal.SizeOf(typeof(T));
            }

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = Math.Max(size,16),
                Usage = ResourceUsage.Dynamic
            };

            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, bd);
        }

        public void Update(DeviceContext ctx,ref T value)
        {      
            DataBox db = ctx.MapSubresource(this.Buffer,0, MapMode.WriteDiscard, MapFlags.None);
            Utilities.Write(db.DataPointer, ref value);
            ctx.UnmapSubresource(this.Buffer, 0);
        }

        public SharpDX.Direct3D11.Buffer Buffer { get; protected set; }

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }

    }
}
