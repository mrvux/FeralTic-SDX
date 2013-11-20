using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using SharpDX.Direct3D11;
using SharpDX.Direct3D;

namespace FeralTic.DX11
{
    /// <summary>
    /// Simple Null device (used for effect reflection)
    /// </summary>
    public static class NullDevice
    {
        private static Device1 device;

        public static Device1 Device
        {
            get
            {
                if (device == null)
                {
                    Device d = new Device(DriverType.Null, DeviceCreationFlags.None);
                    device = d.QueryInterface<Device2>() as Device2;
                }
                return device;
            }
        }
    }
}
