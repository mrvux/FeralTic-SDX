using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class SegmentZBuilder : IGeometryBuilder<SegmentZ>
    {
        public PrimitiveInfo GetPrimitiveInfo(SegmentZ settings)
        {
            return PrimitiveInfo.UnKnown;
        }

        public void Construct(SegmentZ settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            int res = settings.Resolution;
            float cycles = settings.Cycles;
            float phase = settings.Phase;
            float inner = settings.InnerRadius;
            float z = settings.Z;

            int vcount = res * 2;
            int icount = (res - 1) * 6;

            float inc = Convert.ToSingle((Math.PI * 2.0 * cycles) / (res - 1.0));
            float phi = Convert.ToSingle(phase * (Math.PI * 2.0));

            Vector3 n = new Vector3(0.0f, 0.0f, 1.0f);

            #region Append front face
            for (int i = 0; i < res; i++)
            {
                float x = Convert.ToSingle(0.5 * inner * Math.Cos(phi));
                float y = Convert.ToSingle(0.5 * inner * Math.Sin(phi));

                innerv.Position = new Vector4(x, y, z, 1.0f);

                x = Convert.ToSingle(0.5 * Math.Cos(phi));
                y = Convert.ToSingle(0.5 * Math.Sin(phi));

                outerv.Position = new Vector4(x, y, z, 1.0f);

                vertices[i] = innerv;
                vertices[i + res] = outerv;
                phi += inc;
            }

            int indstep = 0;
            int[] indices = new int[icount];
            for (int i = 0; i < res - 1; i++)
            {
                //Triangle from low to high
                indices[indstep] = i;
                indices[indstep + 1] = res + i;
                indices[indstep + 2] = i + 1;


                //Triangle from high to low
                indices[indstep + 3] = i + 1;
                indices[indstep + 4] = res + i;
                indices[indstep + 5] = res + i + 1;

                indstep += 6;
            }

            vlist.AddRange(vertices);
            ilist.AddRange(indices);
            #endregion

            #region Append Back Face
            //Second layer just has Z inverted
            for (int i = 0; i < res * 2; i++)
            {
                vertices[i].Position.Z = -vertices[i].Position.Z;
                vertices[i].Normals.Z = -vertices[i].Normals.Z;
                phi += inc;
            }

            //Here we also flip triangles for cull
            indstep = 0;
            int offset = res * 2;
            for (int i = offset; i < offset + res - 1; i++)
            {
                //Triangle from low to high
                indices[indstep] = i;
                indices[indstep + 2] = res + i;
                indices[indstep + 1] = i + 1;


                //Triangle from high to low
                indices[indstep + 3] = i + 1;
                indices[indstep + 5] = res + i;
                indices[indstep + 4] = res + i + 1;

                indstep += 6;
            }

            vlist.AddRange(vertices);
            ilist.AddRange(indices);
            #endregion
        }
    }
}
