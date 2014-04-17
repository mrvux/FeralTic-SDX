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
        public DX11IndexedGeometry Cylinder(Cylinder settings)
        {
            CylinderBuilder builder = new CylinderBuilder();
            ListGeometryAppender appender = new ListGeometryAppender();
            PrimitiveInfo info = builder.GetPrimitiveInfo(settings);
            builder.Construct(settings, appender.AppendVertex, appender.AppendIndex);
            return FromAppender(settings, appender, info);
        }

        private DX11IndexedGeometry FromAppender(AbstractPrimitiveDescriptor descriptor, ListGeometryAppender appender, PrimitiveInfo info)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.Tag = descriptor;
            geom.PrimitiveType = descriptor.PrimitiveType;
            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable(device, appender.Vertices.ToArray());
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, appender.Indices.ToArray());
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.HasBoundingBox = info.IsBoundingBoxKnown;
            geom.BoundingBox = info.BoundingBox;
            return geom;
        }
    }
}
