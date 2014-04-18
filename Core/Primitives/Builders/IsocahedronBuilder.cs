using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class IsocahedronBuilder : IGeometryBuilder<Isocahedron>
    {
        public PrimitiveInfo GetPrimitiveInfo(Isocahedron settings)
        {
            return new PrimitiveInfo(12, 60,new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1)));
        }

        public void Construct(Isocahedron settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            // This is the golden ratio
            float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;

            Pos3Norm3Tex2Vertex v = new Pos3Norm3Tex2Vertex();

            v.Position = Vector3.Normalize(new Vector3(-1, t, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(1, t, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(-1, -t, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(1, -t, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);

            v.Position = Vector3.Normalize(new Vector3(0, -1, t)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(0, 1, t)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(0, -1, -t)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(0, 1, -t)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);

            v.Position = Vector3.Normalize(new Vector3(t, 0, -1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(t, 0, 1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(-t, 0, -1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(-t, 0, 1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position, v.Normals, v.TexCoords);

            appendIndex(new Int3(0, 11, 5));
            appendIndex(new Int3(0, 5, 1));
            appendIndex(new Int3(0, 1, 7));
            appendIndex(new Int3(0, 7, 10));
            appendIndex(new Int3(0, 10, 11));
            appendIndex(new Int3(1, 5, 9));
            appendIndex(new Int3(5, 11, 4));
            appendIndex(new Int3(11, 10, 2));
            appendIndex(new Int3(10, 7, 6));
            appendIndex(new Int3(7, 1, 8));
            appendIndex(new Int3(3, 9, 4));
            appendIndex(new Int3(3, 4, 2));
            appendIndex(new Int3(3, 2, 6));
            appendIndex(new Int3(3, 6, 8));
            appendIndex(new Int3(3, 8, 9));
            appendIndex(new Int3(4, 9, 5));
            appendIndex(new Int3(2, 4, 11));
            appendIndex(new Int3(6, 2, 10));
            appendIndex(new Int3(8, 6, 7));
            appendIndex(new Int3(9, 8, 1));


        }
    }
}
