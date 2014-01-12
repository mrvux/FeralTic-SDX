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
    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry Tetrahedron(Tetrahedron settings)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;
            geom.InputLayout = Pos3Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;


            DataStream ds = new DataStream(4 * Pos3Norm3Tex2Vertex.VertexSize, false, true);

            Pos3Norm3Tex2Vertex v = new Pos3Norm3Tex2Vertex();

            v.Position = Vector3.Normalize(new Vector3(1, 1, 1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(-1, -1, 1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(1, -1, -1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(-1, 1, -1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            int[] inds = new int[] { 
                0,1,2,
                1,2,3,
                2,3,0,
                3,0,1 };

            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable<Pos3Norm3Tex2Vertex>(device, ds);
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, inds);
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));

            ds.Dispose();


            return geom;
        }
    }
}
