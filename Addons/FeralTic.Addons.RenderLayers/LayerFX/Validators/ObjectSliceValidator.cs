using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public class ObjectSliceValidator : IObjectLayerValidator
    {
        public bool Enabled { get; set; }
        private LayerSettings settings;

        public int Index { get; set; }

        public void SetGlobalSettings(LayerSettings settings)
        {
            this.settings = settings;
        }

        public bool Validate(ObjectLayerSettings obj)
        {
            return obj.DrawCallIndex == this.Index;
        }

        public void Reset()
        {
        }
    }
}
