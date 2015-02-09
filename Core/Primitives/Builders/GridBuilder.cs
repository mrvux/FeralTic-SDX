using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class GridBuilder : IGeometryBuilder<Grid>
    {
        public PrimitiveInfo GetPrimitiveInfo(Grid settings)
        {
            Vector2 size = settings.Size;
            int resX = settings.ResolutionX;
            int resY = settings.ResolutionY;
            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;

            int icount = (resX - 1) * (resY - 1) * 6;

            BoundingBox box = new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));
            return new PrimitiveInfo(resX * resY, icount, box);
        }

        public void Construct(Grid settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            Vector2 size = settings.Size;
            int resX = settings.ResolutionX;
            int resY = settings.ResolutionY;

            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;

            float ix = (sx / Convert.ToSingle(resX - 1)) * 2.0f;
            float iy = (sy / Convert.ToSingle(resY - 1)) * 2.0f;

            float y = -sy;

            Vector3 n = new Vector3(0, 0, -1.0f);

            for (int i = 0; i < resY; i++)
            {
                float x = -sx;
                for (int j = 0; j < resX; j++)
                {
                    Vector2 uv;
                    uv.X = Map((float)j, 0.0f, resX - 1, 0.0f, 1.0f);
                    uv.Y = Map((float)i, 0.0f, resY - 1, 1.0f, 0.0f);
                    appendVertex(new Vector3(x, y, 0.0f), n, uv);
                    x += ix;
                }
                y += iy;
            }

            for (int j = 0; j < resY - 1; j++)
            {
                int rowlow = (j * resX);
                int rowup = ((j + 1) * resX);
                for (int i = 0; i < resX - 1; i++)
                {
                    int col = i * (resX - 1);
                    appendIndex(new Int3(0 + rowlow + i, 0 + rowup + i, 1 + rowlow + i));
                    appendIndex(new Int3(1 + rowlow + i, 0 + rowup + i, 1 + rowup + i));
                }
            }
        }

        private float Map(float Input, float InMin, float InMax, float OutMin, float OutMax)
        {
            float range = InMax - InMin;
            float normalized = (Input - InMin) / range;
            float output = OutMin + normalized * (OutMax - OutMin);
            float min = Math.Min(OutMin, OutMax);
            float max = Math.Max(OutMin, OutMax);
            return Math.Min(Math.Max(output, min), max);
        }
    }
}
