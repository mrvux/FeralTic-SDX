using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX.Direct3D;
using FeralTic.DX11.Utils;

namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry QuadNormals(Quad settings)
        {
            QuadBuilder builder = new QuadBuilder();
            ListGeometryAppender appender = new ListGeometryAppender();
            PrimitiveInfo info = builder.GetPrimitiveInfo(settings);
            builder.Construct(settings, appender.AppendVertex, appender.AppendIndex);
            return FromAppender(settings, appender, info);
        }

        private DX11IndexedGeometry QuadTextured()
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(this.device);
            float sx = 1.0f;
            float sy = 1.0f;
            Pos4Tex2Vertex[] vertices = new Pos4Tex2Vertex[]
            {
                new Pos4Tex2Vertex()
                {
                    Position = new Vector4(-sx, sy, 0.0f, 1.0f),
                    TexCoords = new Vector2(0, 0)
                },
                new Pos4Tex2Vertex()
                {
                    Position = new Vector4(sx, sy, 0.0f, 1.0f),
                    TexCoords = new Vector2(1, 0)
                },
                new Pos4Tex2Vertex()
                {
                    Position = new Vector4(-sx, -sy, 0.0f, 1.0f),
                    TexCoords = new Vector2(0, 1)
                },
                new Pos4Tex2Vertex()
                {
                    Position = new Vector4(sx, -sy, 0.0f, 1.0f),
                    TexCoords = new Vector2(1, 1)
                },
            };
            int[] indices = new int[] { 0, 1, 3, 3, 2, 0 };

            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable(device, vertices); ;
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, indices);
            geom.InputLayout = Pos4Tex2Vertex.Layout;
            geom.VertexBuffer.InputLayout = geom.InputLayout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));

            return geom;
        }

        public DX11VertexGeometry QuadLine(Quad settings)
        {
            Vector2 size = settings.Size;

            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;

            DX11VertexGeometry geom = new DX11VertexGeometry(device);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;
            geom.Topology = PrimitiveTopology.LineStrip;
            geom.InputLayout = Pos4Vertex.Layout;
            geom.VerticesCount = 5;
            geom.VertexSize = Pos4Vertex.VertexSize;
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));

            Vector4[] verts = new Vector4[]
            {
                new Vector4(-sx, -sy, 0.0f, 1.0f),
                new Vector4(sx, -sy, 0.0f, 1.0f),
                new Vector4(sx, sy, 0.0f, 1.0f),
                new Vector4(-sx, sy, 0.0f, 1.0f),
                new Vector4(-sx, -sy, 0.0f, 1.0f)
            };
            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable<Vector4>(device, verts).Buffer;
            return geom;
        }

    }
}
