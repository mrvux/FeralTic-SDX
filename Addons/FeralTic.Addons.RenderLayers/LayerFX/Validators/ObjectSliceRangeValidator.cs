using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public class ObjectSliceRangeValidator : IObjectLayerValidator
    {
        public bool Enabled { get; set; }
        private LayerSettings settings;

        public int MinIndex { get; set; }
        public int MaxIndex { get; set; }

        public void SetGlobalSettings(LayerSettings settings)
        {
            this.settings = settings;
        }

        public bool Validate(ObjectLayerSettings obj)
        {
            return obj.DrawCallIndex >= this.MinIndex && obj.DrawCallIndex <= this.MaxIndex;
        }

        public void Reset()
        {
        }
    }
}
