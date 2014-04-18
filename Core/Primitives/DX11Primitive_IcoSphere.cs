using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Utils;
using FeralTic.DX11.Resources;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry IcoSphere(IcoSphere settings)
        {
            float radius = settings.Radius;
            IcoSphereBuilder builder = new IcoSphereBuilder();
            PrimitiveInfo info = builder.GetPrimitiveInfo(settings);
            ListGeometryAppender appender = new ListGeometryAppender();
            builder.Construct(settings, appender.AppendVertex, appender.AppendIndex);
            DX11IndexedGeometry geom = FromAppender(settings, appender, info);
            geom.BoundingBox = new BoundingBox(new Vector3(-radius), new Vector3(radius));
            return geom;
        }

    }
}
