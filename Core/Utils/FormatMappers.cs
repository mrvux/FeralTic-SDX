using SharpDX;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Utils
{
    public class FormatMappers
    {
        private Dictionary<Type, Format> sdxtoformat = new Dictionary<Type, Format>();
        private Dictionary<Format, Type> formattosdx = new Dictionary<Format, Type>();

        private void RegisterType<T>(Format format) where T : struct
        {
            sdxtoformat.Add(typeof(T), format);
            formattosdx.Add(format, typeof(T));
        }

        public FormatMappers()
        {
            RegisterType<sbyte>(Format.R8_SInt);
            RegisterType<byte>(Format.R8_UInt);

            RegisterType<short>(Format.R16_SInt);
            RegisterType<ushort>(Format.R16_UInt);

            RegisterType<int>(Format.R32_SInt);
            RegisterType<uint>(Format.R32_UInt);

            RegisterType<float>(Format.R32_Float);

            RegisterType<Vector2>(Format.R32G32_Float);
            RegisterType<Vector3>(Format.R32G32B32_Float);
            RegisterType<Vector4>(Format.R32G32B32A32_Float);

            RegisterType<Int3>(Format.R32G32B32_SInt);
            RegisterType<Int4>(Format.R32G32B32A32_SInt);
        }
    }
}
