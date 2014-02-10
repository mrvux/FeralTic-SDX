using FeralTic.DX11;
using FeralTic.RenderLayers.LayerFX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.RenderActions
{
    public static partial class LayerActions
    {
        public static Action<LayerSettings> AttachGeometry(LayerSettings settings, IDxGeometry geometry)
        {
            IDxGeometry geom = settings.Geometry;
            
            settings.Geometry = geom;
            Action<LayerSettings> restore = (rs) => { rs.Geometry = geom; };
            return restore;
        }

        public static Action<LayerSettings> DetachGeometry(LayerSettings settings)
        {
            IDxGeometry geom = settings.Geometry;

            settings.Geometry = null;
            Action<LayerSettings> restore = (rs) => { rs.Geometry = geom; };
            return restore;
        }
    }
}
