using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry Segment(Segment settings)
        {
            SegmentBuilder builder = new SegmentBuilder();
            ListGeometryAppender appender = new ListGeometryAppender();
            PrimitiveInfo info = builder.GetPrimitiveInfo(settings);

            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            builder.Construct(settings, (v, n, u) =>
                { appender.AppendVertex(v, n, u); min = Vector3.Min(min, v); max = Vector3.Max(max, v); }, appender.AppendIndex);

            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;
            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable(device, appender.Vertices.ToArray());
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, appender.Indices.ToArray());
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(min, max);
            return geom;
        }
    }
}
