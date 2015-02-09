using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class TriangleBuilder : IGeometryBuilder<Triangle>
    {
        public PrimitiveInfo GetPrimitiveInfo(Triangle settings)
        {
            return new PrimitiveInfo(3, 3);
        }

        public void Construct(Triangle settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {

            Vector3 e1 = settings.P2 - settings.P1;
            Vector3 e2 = settings.P3 - settings.P2;
            Vector3 n = Vector3.Normalize(Vector3.Cross(e1, e2));

            appendVertex(settings.P1, n, new Vector2(0, 0));
            appendVertex(settings.P2, n, new Vector2(1, 0));
            appendVertex(settings.P3, n, new Vector2(0, 1));
 
            appendIndex(new Int3(0, 1, 2));
        }
    }
}
