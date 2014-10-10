using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class TetrahedronBuilder : IGeometryBuilder<Tetrahedron>
    {
        public PrimitiveInfo GetPrimitiveInfo(Tetrahedron settings)
        {
            return new PrimitiveInfo(4, 12,new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1)));
        }

        public void Construct(Tetrahedron settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            // This is the golden ratio
            float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;

            Pos3Norm3Tex2Vertex v = new Pos3Norm3Tex2Vertex();

            v.Position = Vector3.Normalize(new Vector3(1, 1, 1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position* settings.Size, v.Normals, v.TexCoords);

            v.Position = Vector3.Normalize(new Vector3(-1, -1, 1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position * settings.Size, v.Normals, v.TexCoords);

            v.Position = Vector3.Normalize(new Vector3(1, -1, -1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position * settings.Size, v.Normals, v.TexCoords);

            v.Position = Vector3.Normalize(new Vector3(-1, 1, -1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position * settings.Size, v.Normals, v.TexCoords);

            appendIndex(new Int3(0,1,2));
            appendIndex(new Int3(1,2,3));
            appendIndex(new Int3(2,3,0));
            appendIndex(new Int3(3,0,1));
        }
    }
}
