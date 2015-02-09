using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class DodecahedronBuilder : IGeometryBuilder<Dodecahedron>
    {
        public PrimitiveInfo GetPrimitiveInfo(Dodecahedron settings)
        {
            return PrimitiveInfo.UnKnown;
        }

        public void Construct(Dodecahedron settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            // This is the golden ratio
            float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;

            float a = 1.0f / (float)Math.Sqrt(3.0);
            float b = 0.356822089773089931942f; // sqrt( ( 3 - sqrt(5) ) / 6 )
            float c = 0.934172358962715696451f; // sqrt( ( 3 + sqrt(5) ) / 6 );

            Vector3 size = settings.Size;
            Vector2 uv = Vector2.Zero;

            Vector3[] verts = new Vector3[]
            {
                new Vector3( a,  a,  a ),
                new Vector3(  a,  a, -a ),
                new Vector3(  a, -a,  a ),
                new Vector3(  a, -a, -a ),
                new Vector3( -a,  a,  a ),
                new Vector3( -a,  a, -a ),
                 new Vector3( -a, -a,  a ),
                 new Vector3( -a, -a, -a ),
                  new Vector3( b,  c,  0 ),
                 new Vector3( -b,  c,  0 ),
                 new Vector3(  b, -c,  0 ),
                 new Vector3( -b, -c,  0 ),
                 new Vector3(  c,  0,  b ),
                 new Vector3(  c,  0, -b ),
                 new Vector3( -c,  0,  b ),
                 new Vector3( -c,  0, -b ),
                 new Vector3(  0,  b,  c ),
                 new Vector3(  0, -b,  c ),
                 new Vector3(  0,  b, -c ),
                 new Vector3(  0, -b, -c )
            };

            int[] faces = new int[]
            {
                0, 8, 9, 4, 16,
                0, 16, 17, 2, 12,
                12, 2, 10, 3, 13,
                9, 5, 15, 14, 4,
                3, 19, 18, 1, 13,
                7, 11, 6, 14, 15,
                0, 12, 13, 1, 8,
                8, 1, 18, 5, 9,
                16, 4, 14, 6, 17,
                6, 11, 10, 2, 17,
                7, 15, 5, 18, 19,
                7, 19, 3, 10, 11,
            };

            int cnt = 0;

            for (int j = 0; j < faces.Length; j += 5, ++t)
            {
                int v0 = faces[j];
                int v1 = faces[j + 1];
                int v2 = faces[j + 2];
                int v3 = faces[j + 3];
                int v4 = faces[j + 4];

                Vector3 normal = Vector3.Cross(verts[v1] - verts[v0], verts[v2] - verts[v0]);
                normal = Vector3.Normalize(normal);

                appendIndex(new Int3(cnt, cnt + 1, cnt + 2));
                appendIndex(new Int3(cnt, cnt + 2, cnt + 3));
                appendIndex(new Int3(cnt, cnt + 3, cnt + 4));

                cnt += 5;

                appendVertex(verts[v0] * settings.Size, normal, uv);
                appendVertex(verts[v1] * settings.Size, normal, uv);
                appendVertex(verts[v2] * settings.Size, normal, uv);
                appendVertex(verts[v3] * settings.Size, normal, uv);
                appendVertex(verts[v4] * settings.Size, normal, uv);
            }

        }
    }
}
