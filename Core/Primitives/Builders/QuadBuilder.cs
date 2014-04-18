using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class QuadBuilder : IGeometryBuilder<Quad>
    {
        public PrimitiveInfo GetPrimitiveInfo(Quad settings)
        {
            Vector2 size = settings.Size;

            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;
            return new PrimitiveInfo(4, 6, new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));
        }

        public void Construct(Quad settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            Vector2 size = settings.Size;

            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;

            Vector3 n = new Vector3(0, 0, 1);

            appendVertex(new Vector3(-sx, sy, 0.0f), n, new Vector2(0, 0));
            appendVertex(new Vector3(sx, sy, 0.0f), n, new Vector2(1, 0));
            appendVertex(new Vector3(-sx, -sy, 0.0f), n, new Vector2(0, 1));
            appendVertex(new Vector3(sx, sy, 0.0f), n, new Vector2(1,1));
                   
            appendIndex(new Int3(0, 1, 3));
            appendIndex(new Int3(3,2,0));
        }
    }
}
