using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class SphereBuilder : IGeometryBuilder<Sphere>
    {
        public PrimitiveInfo GetPrimitiveInfo(Sphere settings)
        {
            return PrimitiveInfo.UnKnown;
        }

        public void Construct(Sphere settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            float radius = settings.Radius;
            int resX = settings.ResX;
            int resY = settings.ResY;
            float cx = settings.CyclesX;
            float cy = settings.CyclesY;

            float pi = (float)Math.PI;
            float pidiv2 = pi * 0.5f;
            float twopi = pi * 2.0f;

            for (int i = 0; i <= resY; i++)
            {
                float v = 1 - (float)i / resY;

                float latitude = (i * pi / resY) - pidiv2;

                float dy = (float)Math.Sin(latitude * cy);
                float dxz = (float)Math.Cos(latitude * cy);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j <= resX; j++)
                {
                    float u = (float)j / resX;

                    float longitude = j * twopi / resX;

                    float dx = (float)Math.Sin(longitude * cx);
                    float dz = (float)Math.Cos(longitude * cx);

                    dx *= dxz;
                    dz *= dxz;

                    Vector3 p = new Vector3(dx * radius, dy * radius, dz * radius);
                    Vector3 n = Vector3.Normalize(p);
                    Vector2 uv = new Vector2(u, v);

                    appendVertex(p, n, uv);
                }
            }


            int stride = resX + 1;

            for (int i = 0; i < resY; i++)
            {
                for (int j = 0; j <= resX; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % stride;

                    appendIndex(new Int3(i * stride + j, i * stride + nextJ, nextI * stride + j));
                    appendIndex(new Int3(i * stride + nextJ, nextI * stride + nextJ, nextI * stride + j));
                }
            }
        }
    }
}
