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
            PrimitiveInfo info = builder.GetPrimitiveInfo(settings);
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

            return FromAppender(settings, appender, info);
        }
    }
}
