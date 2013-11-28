using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX;

using FeralTic.DX11.Resources;
using FeralTic.DX11.Utils;
using SharpDX.Direct3D;


namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager
    {
        public DX11IndexedGeometry Octahedron(Octahedron settings)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.PrimitiveType = settings.PrimitiveType;
            geom.Tag = settings;
            geom.InputLayout = Pos3Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;

            DataStream ds = new DataStream(6 * Pos3Norm3Tex2Vertex.VertexSize, false, true);

            Pos3Norm3Tex2Vertex v = new Pos3Norm3Tex2Vertex();

            Vector3 size = settings.Size;

            v.Position = Vector3.Normalize(new Vector3(1.0f,0,0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(-1.0f,0,0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(0,1.0f,0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(0,-1.0f,0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            v.Position = Vector3.Normalize(new Vector3(0,0,-1.0f)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(0,0, 1.0f)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            int[] inds = new int[] { 
                0,4,2,
                0,2,5, 
                0,3,4,
                0,5,3,
                1,2,4,
                1,5,2,
                1,4,3,
                1,3,5 };


            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable<Pos3Norm3Tex2Vertex>(device, ds);
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, inds);
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));

            ds.Dispose();
            return geom;
        }
    }
}
