using FeralTic.DX11;
using FeralTic.DX11.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMallConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DxDevice dev = new DxDevice(SharpDX.Direct3D11.DeviceCreationFlags.Debug);

            DX11VertexBuffer vbo = DX11VertexBuffer.CreateWriteable(dev, 16, 16, eVertexBufferWriteMode.None);
        }
    }
}
