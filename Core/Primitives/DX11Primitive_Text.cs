using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX.Direct3D;
using SharpDX.Direct2D1;

namespace FeralTic.DX11.Geometry
{
    internal class TextTesselator : TessellationSink
    {
        private List<Pos3Norm3Tex2Vertex> verts = new List<Pos3Norm3Tex2Vertex>();

        private Vector3 norm = new Vector3(0, 0, -1);
        private Vector2 uv = new Vector2(0, 0);

        private Pos3Norm3Tex2Vertex vertex;

        public TextTesselator()
        {
            vertex = new Pos3Norm3Tex2Vertex()
            {
                Position = Vector3.Zero,
                Normals = norm,
                TexCoords=uv
            };
        }
        

        public List<Pos3Norm3Tex2Vertex> Vertices
        {
            get { return verts; }
        }


        public void AddTriangles(Triangle[] triangles)
        {
            foreach (Triangle tri in triangles)
            {
                vertex.Position = new Vector3(tri.Point1.X,tri.Point1.Y,0.0f);
                verts.Add(vertex);

                vertex.Position = new Vector3(tri.Point3.X, tri.Point3.Y, 0.0f);
                verts.Add(vertex);

                vertex.Position = new Vector3(tri.Point2.X, tri.Point2.Y, 0.0f);
                verts.Add(vertex);
            }
        }

        public void Close()
        {
           
        }

        public IDisposable Shadow
        {
            get;
            set;
        }

        public void Dispose()
        {

        }
    }

    public partial class PrimitivesManager
    {
        public DX11VertexGeometry Text(TextPrimitive settings)
        {
            /*var path = new System.Drawing.Drawing2D.GraphicsPath();
            var ftn = new System.Drawing.FontFamily("Arial");
            var str = settings.Text;

            path.FillMode = System.Drawing.Drawing2D.FillMode.Winding;

            System.Drawing.StringFormat fmt = System.Drawing.StringFormat.GenericDefault;
            fmt.Alignment = System.Drawing.StringAlignment.Near;

            path.AddString(str, ftn, 0, 256, new System.Drawing.PointF(0, 0), fmt);
            path.Flatten();

            var pts = path.PathPoints;
            var typ = path.PathTypes;

            PathGeometry d2dpath = new PathGeometry(this.device.D2DFactory);
            var sink = d2dpath.Open();

            for (int i = 0; i < pts.Length; i++)
            {
                Vector2 v = new Vector2(pts[i].X, pts[i].Y);
                if (typ[i] == 0)
                {
                    sink.BeginFigure(v, FigureBegin.Filled);
                }

                if (typ[i] == 1)
                {
                    sink.AddLine(v);
                }

                if (typ[i] == 129 || typ[i] == 161)
                {
                    sink.AddLine(v);
                    sink.EndFigure(FigureEnd.Closed);
                }
            }
            sink.Close();

            TextTesselator tessel = new TextTesselator();
            d2dpath.Tessellate(0.2f, tessel);

            DX11VertexGeometry geom = new DX11VertexGeometry(this.device);
            geom.InputLayout = Pos3Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VertexSize = Pos3Norm3Tex2Vertex.VertexSize;
            geom.VerticesCount = tessel.Vertices.Count;
            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable<Pos3Norm3Tex2Vertex>(this.device, tessel.Vertices.ToArray()).Buffer;

            return geom;*/
            return null;
        }
    }
}
