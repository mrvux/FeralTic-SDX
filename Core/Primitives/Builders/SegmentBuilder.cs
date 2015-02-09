using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class SegmentBuilder : IGeometryBuilder<Segment>
    {
        public PrimitiveInfo GetPrimitiveInfo(Segment settings)
        {
            float phase = settings.Phase;
            float cycles = settings.Cycles;
            float inner = settings.InnerRadius;
            int res = settings.Resolution;
            bool flat = settings.Flat;

            int vcount = res * 2;
            int icount = (res - 1) * 6;

            return new PrimitiveInfo(vcount, icount);
        }

        public void Construct(Segment settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            float phase = settings.Phase;
            float cycles = settings.Cycles;
            float inner = settings.InnerRadius;
            int res = settings.Resolution;
            bool flat = settings.Flat;

            int vcount = res * 2;
            int icount = (res - 1) * 6;

            float inc = Convert.ToSingle((Math.PI * 2.0 * cycles) / (res - 1.0));
            float phi = Convert.ToSingle(phase * (Math.PI * 2.0));

            Pos3Norm3Tex2Vertex innerv = new Pos3Norm3Tex2Vertex();
            innerv.Normals = new Vector3(0.0f, 0.0f, 1.0f);

            Pos3Norm3Tex2Vertex outerv = new Pos3Norm3Tex2Vertex();
            outerv.Normals = new Vector3(0.0f, 0.0f, 1.0f);

            Pos4Norm3Tex2Vertex[] vertices = new Pos4Norm3Tex2Vertex[res * 2];

            for (int i = 0; i < res; i++)
            {
                float x = Convert.ToSingle(0.5 * inner * Math.Cos(phi));
                float y = Convert.ToSingle(0.5 * inner * Math.Sin(phi));

                innerv.Position = new Vector3(x, y, 0.0f);
                if (flat)
                {
                    innerv.TexCoords = new Vector2(0.5f - x, 0.5f - y);
                }
                else
                {
                    innerv.TexCoords = new Vector2((1.0f * (float)i) / ((float)res - 1.0f), 0.0f);
                }
                appendVertex(innerv.Position, innerv.Normals, innerv.TexCoords);
                phi += inc;
            }

            phi = Convert.ToSingle(phase * (Math.PI * 2.0));
            for (int i = 0; i < res; i++)
            {
                float x = Convert.ToSingle(0.5 * Math.Cos(phi));
                float y = Convert.ToSingle(0.5 * Math.Sin(phi));
                outerv.Position = new Vector3(x, y, 0.0f);

                if (flat)
                {
                    outerv.TexCoords = new Vector2(0.5f - x, 0.5f - y);
                }
                else
                {
                    outerv.TexCoords = new Vector2((1.0f * (float)i) / ((float)res - 1.0f), 1.0f);
                }

                appendVertex(outerv.Position, outerv.Normals, outerv.TexCoords);
                phi += inc;
            }



            for (int i = 0; i < res - 1; i++)
            {
                appendIndex(new Int3(i, i + 1, res+ i));
                appendIndex(new Int3(i+1,res + i + 1, res + i));
            }
        }
    }
}
