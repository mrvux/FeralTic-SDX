using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Primitives
{
    public class QuadBuilder
    {
        public static Vector4[] BuildPositions(Quad quad)
        {
            float sx = 0.5f * quad.Size.X;
            float sy = 0.5f * quad.Size.Y;

            Vector4[] positions = new Vector4[]
            {
                new Vector4(-sx, sy, 0.0f, 1.0f),
                new Vector4(sx, sy, 0.0f, 1.0f),
                new Vector4(-sx, -sy, 0.0f, 1.0f),
                new Vector4(sx, -sy, 0.0f, 1.0f),
            };
            return positions;
        }

        public static Vector3[] BuildNormals(Quad quad)
        {
            Vector3[] normals = new Vector3[]
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
            };
            return normals;
        }

        public static Vector2[] BuildUVs(Quad quad)
        {
            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            };
            return uvs;
        }

        public static uint[] BuildIndices()
        {
            return new uint[] { 0, 1, 3, 3, 2, 0 };
        }

        public static void BuildPosition(DX11Device device, Quad quad, out DX11VertexBuffer vbo)
        {
            vbo = DX11VertexBuffer.CreateImmutable<Vector4>(device, BuildPositions(quad));
        }

        public static void BuildPosition(DX11Device device, Quad quad, out DX11StructuredBuffer sb)
        {
            sb = DX11StructuredBuffer.CreateImmutable<Vector4>(device, BuildPositions(quad));
        }

        public static void BuildIndices(DX11Device device, Quad quad, out DX11IndexBuffer ibo)
        {
            ibo = DX11IndexBuffer.CreateImmutable(device, BuildIndices());
        }

        public static void BuildIndices(DX11Device device, Quad quad, out DX11StructuredBuffer ibo)
        {
            uint[] inds = new uint[] { 0, 1, 3, 3, 2, 0 };
            ibo = DX11StructuredBuffer.CreateImmutable(device, inds);
        }
    }
}
