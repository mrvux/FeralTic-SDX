using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public interface IObjectLayerValidator
    {
        void Reset();
        void SetGlobalSettings(LayerSettings settings);
        bool Validate(ObjectLayerSettings obj);
        bool Enabled { get; }
    }
}
