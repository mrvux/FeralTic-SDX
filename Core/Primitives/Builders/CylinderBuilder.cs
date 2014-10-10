using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class CylinderBuilder : IGeometryBuilder<Cylinder>
    {
        public PrimitiveInfo GetPrimitiveInfo(Cylinder settings)
        {

            float radius1 = settings.Radius1;
            float radius2 = settings.Radius2;
            float cycles = settings.Cycles;
            float length = settings.Length;
            int resX = settings.ResolutionX;
            int resY = settings.ResolutionY;
            bool caps = settings.Caps;
            int vcount = resX * (resY + 1);
            int icount = vcount * 6;

            if (caps)
            {
                //Add vertices (+1 for center)
                vcount += (resX + 1 * 2);

                //Add Triangles (on each side
                icount += (resX * 6);
            }

            float lenstart = -length * 0.5f; //Start at half bottom
            float lenstep = (float)length / (float)resY;
            float maxrad = radius1 > radius2 ? radius1 : radius2;

            BoundingBox box = new BoundingBox(new Vector3(-maxrad, -lenstart, -maxrad), new Vector3(maxrad, lenstart, maxrad));
            
            return new PrimitiveInfo(vcount, icount, box);
        }

        public void Construct(Cylinder settings, Action<Vector3,Vector3,Vector2> appendVertex, Action<Int3> appendIndex)
        {
            float radius1 = settings.Radius1;
            float radius2 = settings.Radius2;
            float cycles = settings.Cycles;
            float length = settings.Length;
            int resX = settings.ResolutionX;
            int resY = settings.ResolutionY;
            bool caps = settings.Caps;

            float lenstart = settings.Center ? -length * 0.5f : 0.0f; //Start at half bottom
            float lenstep = (float)length / (float)resY;

            float y = lenstart;

            float phi = 0.0f;
            float inc = Convert.ToSingle((Math.PI * 2.0 * cycles) / (double)resX);

            float fres = Convert.ToSingle(resY);
            float radiusRange = radius2 - radius1;

            int vcount = 0;

            #region Add Vertices tube
            for (int i = 0; i < resY + 1; i++)
            {

                float ystep = (float)i / fres;

                float radius = ystep * radiusRange + radius1;

                for (int j = 0; j < resX; j++)
                {
                    float x = Convert.ToSingle(radius1 * Math.Cos(phi)) * radius;
                    float z = Convert.ToSingle(radius1 * Math.Sin(phi)) * radius;

                    appendVertex(new Vector3(x,y,z), Vector3.Normalize(new Vector3(x, 0.0f, z)), Vector2.Zero);
                    vcount++;
                    phi += inc;
                }
                y += lenstep;
                phi = 0.0f;
            }
            #endregion

            #region Add Indices Tube
            int indstart;
            for (int i = 0; i < resY; i++)
            {
                indstart = resX * i;
                int j;
                for (j = 0; j < resX - 1; j++)
                {
                    appendIndex(new Int3(indstart + j, indstart + resX + j, indstart + j + 1));
                    appendIndex(new Int3(indstart + j + 1, indstart + j + resX, indstart + j + resX + 1));
                }

                appendIndex(new Int3(indstart + j, indstart + resX + j, indstart));
                appendIndex(new Int3(indstart + j + resX, indstart + resX, indstart));
            }

            if (caps)
            {
                indstart = vcount;
                y = -length * 0.5f;

                Vector3 n = new Vector3(0, -1, 0);
                appendVertex(new Vector3(0, y, 0),n, Vector2.Zero);

                phi = 0.0f;

                for (int j = 0; j < resX; j++)
                {
                    float x = Convert.ToSingle(radius1 * Math.Cos(phi)) * radius1;
                    float z = Convert.ToSingle(radius1 * Math.Sin(phi)) * radius1;
                    appendVertex(new Vector3(x,y,z), n, Vector2.Zero);
                    phi += inc;
                }

                for (int j = 1; j < resX + 1; j++)
                {
                    int lastidx = j == resX ? indstart + 1 : indstart + j + 1;
                    appendIndex(new Int3(indstart, indstart + j, lastidx));
                }

                indstart += (resX + 1);
                y = length * 0.5f;

                n = new Vector3(0, 1, 0);
                appendVertex(new Vector3(0, y, 0), n, Vector2.Zero);

                phi = 0.0f;

                for (int j = 0; j < resX; j++)
                {
                    float x = Convert.ToSingle(radius1 * Math.Cos(phi)) * radius2;
                    float z = Convert.ToSingle(radius1 * Math.Sin(phi)) * radius2;
                    appendVertex(new Vector3(x, y, z), n, Vector2.Zero);
                    phi += inc;
                }

                for (int j = 1; j < resX + 1; j++)
                {
                    int lastidx = j == resX ? indstart + 1 : indstart + j + 1;
                    appendIndex(new Int3(indstart + j, indstart, lastidx));
                }
            }
            #endregion
        }
    }
}
