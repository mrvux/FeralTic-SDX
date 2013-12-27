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
    public partial class DX11PrimitivesManager
    {
        public DX11NullGeometry NullDrawer(NullGeometry settings)
        {
            DX11NullInstancedDrawer drawer = new DX11NullInstancedDrawer();
            drawer.VertexCount = Math.Max(settings.VertexCount, 1);
            drawer.InstanceCount = Math.Max(settings.InstanceCount, 1);

            DX11NullGeometry geom = new DX11NullGeometry(this.device, drawer);
            geom.Topology = settings.Topology;
            geom.InputLayout = new InputElement[0];
            geom.HasBoundingBox = false;

            return geom;
        }

        public DX11NullGeometry Dispatcher(Dispatcher settings)
        {
            DX11NullDispatcher disp = new DX11NullDispatcher();
            disp.X = Math.Max(settings.ThreadCountX, 0);
            disp.Y = Math.Max(settings.ThreadCountY, 0);
            disp.Z = Math.Max(settings.ThreadCountZ, 0);

            DX11NullGeometry geom = new DX11NullGeometry(this.device, disp);

            geom.Topology = PrimitiveTopology.Undefined;
            geom.InputLayout = new InputElement[0];
            geom.HasBoundingBox = false;
            return geom;
        }
    }

}
