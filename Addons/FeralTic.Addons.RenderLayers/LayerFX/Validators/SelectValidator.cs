using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public class SelectValidator : IObjectLayerValidator
    {
        public bool Enabled { get; set; }
        private LayerSettings settings;

        public IList<bool> Selection { get; set; }

        public void SetGlobalSettings(LayerSettings settings)
        {
            this.settings = settings;
        }

        public bool Validate(ObjectLayerSettings obj)
        {
            return this.Selection[obj.DrawCallIndex];
        }

        public void Reset()
        {
        }
    }
}
