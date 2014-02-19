using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry Polygon2d(Polygon2d settings)
        {

            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;

            int count = settings.Vertices.Length;

            Pos4Norm3Tex2Vertex[] verts = new Pos4Norm3Tex2Vertex[count + 1];

            float cx = 0;
            float cy = 0;
            float x = 0, y = 0;

            float minx = float.MaxValue, miny = float.MaxValue;
            float maxx = float.MinValue, maxy = float.MinValue;

            for (int j = 0; j < count; j++)
            {
                verts[j + 1].Position = new Vector4(settings.Vertices[j].X, settings.Vertices[j].Y, 0.0f, 1.0f);
                verts[j + 1].Normals = new Vector3(0, 0, 1);
                verts[j + 1].TexCoords = new Vector2(0.0f, 0.0f);
                cx += x;
                cy += y;

                if (x < minx) { minx = x; }
                if (x > maxx) { maxx = x; }
                if (y < miny) { miny = y; }
                if (y > maxy) { maxy = y; }
            }

            verts[0].Position = new Vector4(cx / (float)count, cy / (float)count, 0.0f, 1.0f);
            verts[0].Normals = new Vector3(0, 0, 1);
            verts[0].TexCoords = new Vector2(0.5f, 0.5f);

            float w = maxx - minx;
            float h = maxy - miny;
            for (int j = 0; j < count; j++)
            {
                verts[0].TexCoords = new Vector2((verts[j + 1].Position.X - minx) / w, (verts[j + 1].Position.Y - miny) / h);
            }

            List<int> inds = new List<int>();

            for (int j = 0; j < count - 1; j++)
            {
                inds.Add(0);
                inds.Add(j + 1);
                inds.Add(j + 2);
            }

            inds.Add(0);
            inds.Add(verts.Length - 1);
            inds.Add(1);

            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable(device, verts);
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, inds.ToArray());
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;


            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox()
            {
                Minimum = new Vector3(minx, miny, 0.0f),
                Maximum = new Vector3(maxx, maxy, 0.0f)
            };

            return geom;
        }
    }
}
