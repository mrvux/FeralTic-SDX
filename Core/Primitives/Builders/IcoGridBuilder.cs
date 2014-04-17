using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class IcoGridBuilder : IGeometryBuilder<IcoGrid>
    {
        public PrimitiveInfo GetPrimitiveInfo(IcoGrid settings)
        {
            return PrimitiveInfo.UnKnown;
        }

        private float maxx;
        private float maxy;

        public float MaxX
        {
            get { return this.maxx; }
        }

        public float MaxY
        {
            get { return this.maxy; }
        }


        public void Construct(IcoGrid settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            Vector2 size = settings.Size;
            int resX = settings.ResolutionX;
            int resY = settings.ResolutionY;

            float fdispy = (float)Math.Sqrt(0.75);
            Vector3 p;
            Vector3 n =  new Vector3(0, 0, -1);
            Vector2 uv = Vector2.Zero;

            maxx = float.MinValue;
            maxy = float.MinValue;

            bool bdisp = true;
            float y = 0.0f;
            for (int i = 0; i < resX; i++)
            {
                bdisp = !bdisp;

                int cnt = resY;

                for (int j = 0; j < cnt; j++)
                {
                    if (bdisp)
                    {
                        p = new Vector3((float)j - 0.5f, y, 0.0f);

                        if (p.X > maxx)
                        {
                            maxx = p.X;
                        }
                        if (p.Y > maxy)
                        {
                            maxy = p.Y;
                        }
                    }
                    else
                    {
                        p = new Vector3((float)j, y, 0.0f);

                        if (p.X > maxx)
                        {
                            maxx = p.X;
                        }
                        if (p.Y > maxy)
                        {
                            maxy = p.Y;
                        }
                    }
                    appendVertex(p, n, uv);
                }
                y += fdispy;
            }

            bool bflip = true;
            int nextrow = resY;
            int a = 1;
            for (int i = 0; i < resX - 1; i++)
            {
                bflip = !bflip;
                for (int j = 0; j < resY - 1; j++)
                {
                    int linestart = i * resY;
                    int lineup = (i + 1) * resY;
                    if (!bflip)
                    {
                        appendIndex(new Int3(lineup + j,linestart + j,lineup + j + 1));
                        appendIndex(new Int3(linestart + j, linestart + j + 1, lineup + j + 1));
                    }
                    else
                    {
                        appendIndex(new Int3(linestart + j, linestart + j + 1, lineup + j));
                        appendIndex(new Int3(lineup + j, linestart + j + 1, lineup + j + 1));
                    }
                }
                a += nextrow;
            }
        }
    }
}
