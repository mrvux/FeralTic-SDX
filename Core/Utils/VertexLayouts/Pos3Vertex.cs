using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SharpDX;
using SharpDX.Direct3D11;

namespace FeralTic.DX11.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Pos3Vertex
    {
        public Vector3 Position;

        private static InputElement[] layout;

        public static InputElement[] Layout
        {
            get
            {
                if (layout == null)
                {
                    layout = new InputElement[]
                    {
                        new InputElement("POSITION",0,SharpDX.DXGI.Format.R32G32B32_Float,0, 0)
                    };
                }
                return layout;
            }
        }

        public static int VertexSize
        {
            get { return Marshal.SizeOf(typeof(Pos3Vertex)); }
        }
    }
}
