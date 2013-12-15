using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using SharpDX.Direct3D11;
using SharpDX.Direct3D;

#if DIRECTX11_2
using DirectXDevice = SharpDX.Direct3D11.Device2;
#else
#if DIRECTX11_1
using DirectXDevice = SharpDX.Direct3D11.Device1;
#else
using DirectXDevice = SharpDX.Direct3D11.Device;
#endif
#endif

namespace FeralTic.DX11
{
    /// <summary>
    /// Simple Null device (used for effect reflection)
    /// </summary>
    public static class NullDevice
    {
        private static DirectXDevice device;

        public static DirectXDevice Device
        {
            get
            {
                if (device == null)
                {
                    Device d = new Device(DriverType.Null, DeviceCreationFlags.None);
                    #if DIRECTX11_1
                    device = d.QueryInterface<DirectXDevice>();
                    #endif               
                }
                return device;
            }
        }
    }
}
