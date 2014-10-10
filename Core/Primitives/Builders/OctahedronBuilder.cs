using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class OctahedronBuilder : IGeometryBuilder<Octahedron>
    {
        public PrimitiveInfo GetPrimitiveInfo(Octahedron settings)
        {
            return new PrimitiveInfo(6, 24, new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1)));
        }

        public void Construct(Octahedron settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            // This is the golden ratio
            float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;

            Pos3Norm3Tex2Vertex v = new Pos3Norm3Tex2Vertex();

            v.Position = Vector3.Normalize(new Vector3(1.0f, 0, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position*settings.Size, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(-1.0f, 0, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position * settings.Size, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(0, 1.0f, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position * settings.Size, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(0, -1.0f, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position * settings.Size, v.Normals, v.TexCoords);

            v.Position = Vector3.Normalize(new Vector3(0, 0, -1.0f)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position * settings.Size, v.Normals, v.TexCoords);
            v.Position = Vector3.Normalize(new Vector3(0, 0, 1.0f)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0);
            appendVertex(v.Position * settings.Size, v.Normals, v.TexCoords);

            appendIndex(new Int3(0, 4, 2));
            appendIndex(new Int3(0, 2, 5));
            appendIndex(new Int3(0, 3, 4));
            appendIndex(new Int3(0, 5, 3));
            appendIndex(new Int3(1, 2, 4));
            appendIndex(new Int3(1, 5, 2));
            appendIndex(new Int3(1, 4, 3));
            appendIndex(new Int3(1, 3, 5));
        }
    }
}
