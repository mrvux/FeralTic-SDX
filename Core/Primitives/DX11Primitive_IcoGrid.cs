using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using FeralTic.DX11.Utils;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry IcoGrid(IcoGrid settings)
        {
            IcoGridBuilder builder = new IcoGridBuilder();
            ListGeometryAppender appender = new ListGeometryAppender();
            builder.Construct(settings, appender.AppendVertex, appender.AppendIndex);

            float invmx = 1.0f / builder.MaxX;
            float invmy = 1.0f / builder.MaxY;
            appender.TransformVertices( (v) =>
                {
                    v.Position.X *= invmx;
                    v.Position.X -= 0.5f;
                    v.Position.Y *= invmy;
                    v.Position.Y -= 0.5f;
                    return v;
                });

            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;
            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable<Pos4Norm3Tex2Vertex>(device, appender.Vertices.ToArray());
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, appender.Indices.ToArray());
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
 
            geom.HasBoundingBox = false;

            return geom;
        }
    }
}
