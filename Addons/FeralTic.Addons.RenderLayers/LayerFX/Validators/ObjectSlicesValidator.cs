using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public class ObjectSlicesValidator : IObjectLayerValidator
    {
        public bool Enabled { get; set; }
        private LayerSettings settings;

        public List<int> Index { get; set; }

        public void SetGlobalSettings(LayerSettings settings)
        {
            this.settings = settings;
            this.Index = new List<int>();
        }

        public bool Validate(ObjectLayerSettings obj)
        {
            return this.Index.Contains(obj.DrawCallIndex);
        }

        public void Reset()
        {
        }
    }
}
