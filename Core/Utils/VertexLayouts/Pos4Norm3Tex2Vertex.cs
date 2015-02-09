﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

using SharpDX;
using SharpDX.Direct3D11;

namespace FeralTic.DX11.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Pos4Norm3Tex2Vertex
    {
        public Vector4 Position;
        public Vector3 Normals;
        public Vector2 TexCoords;



        private static InputElement[] layout;

        public static InputElement[] Layout
        {
            get
            {
                if (layout == null)
                {
                    layout = new InputElement[]
                    {
                        new InputElement("POSITION",0,SharpDX.DXGI.Format.R32G32B32A32_Float,0, 0),
                        new InputElement("NORMAL",0,SharpDX.DXGI.Format.R32G32B32_Float,16,0),
                        new InputElement("TEXCOORD",0,SharpDX.DXGI.Format.R32G32_Float,28,0),
                    };
                }
                return layout;
            }
        }

        public static int VertexSize
        {
            get { return Marshal.SizeOf(typeof(Pos4Norm3Tex2Vertex)); }
        }
    }
}
