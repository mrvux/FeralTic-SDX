using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.DXGI;
using Fmt = SharpDX.DXGI.Format;

namespace FeralTic.DX11
{
    public enum eDepthFormat 
    {
        d32 = Fmt.R32_Typeless,
        d24s8 = Fmt.R24G8_Typeless,
        d16 = Fmt.R16_Typeless,
        d32s8 = Fmt.R32_Float_X8X24_Typeless
    }

    /// <summary>
    /// Static class to provide format matching for depth.
    /// </summary>
    public static class DepthFormatsExtensions
    {
        public static Format GetGenericTextureFormat(this eDepthFormat depthformat)
        {
            switch (depthformat)
            {
                case eDepthFormat.d32:
                    return Format.R32_Typeless;
                case eDepthFormat.d24s8:
                    return Format.R24G8_Typeless;
                case eDepthFormat.d16:
                    return Format.R16_Typeless;
                case eDepthFormat.d32s8:
                    return Format.R32_Float_X8X24_Typeless;
                default:
                    return Format.R32_Typeless; //Defaults as R32
            }
        }

        public static Format GetDepthFormat(this eDepthFormat depthformat)
        {
            switch (depthformat)
            {
                case eDepthFormat.d32:
                    return Format.D32_Float;
                case eDepthFormat.d24s8:
                    return Format.D24_UNorm_S8_UInt;
                case eDepthFormat.d16:
                    return Format.D16_UNorm;
                case eDepthFormat.d32s8:
                    return Format.D32_Float_S8X24_UInt;
                default:
                    return Format.D32_Float; //Defaults as R32
            }
        }

        public static Format GetSRVFormat(this eDepthFormat depthformat)
        {
            switch (depthformat)
            {
                case eDepthFormat.d32:
                    return Format.R32_Float;
                case eDepthFormat.d24s8:
                    return Format.R24_UNorm_X8_Typeless;
                case eDepthFormat.d16:
                    return Format.R16_UNorm;
                case eDepthFormat.d32s8:
                    return Format.R32_Float_X8X24_Typeless;
                default:
                    return Format.R32_Float;
            }
        }

        public static Format GetStencilSRVFormat(this eDepthFormat depthformat)
        {
            switch (depthformat)
            {
                case eDepthFormat.d32:
                    return Format.Unknown;
                case eDepthFormat.d24s8:
                    return Format.X24_Typeless_G8_UInt;
                case eDepthFormat.d16:
                    return Format.Unknown;
                case eDepthFormat.d32s8:
                    return Format.X32_Typeless_G8X24_UInt;
                default:
                    return Format.Unknown;
            }
        }

        public static bool HasStencil(this eDepthFormat depthformat)
        {
            return depthformat == eDepthFormat.d24s8 || depthformat == eDepthFormat.d32s8;
        }
    }
}
