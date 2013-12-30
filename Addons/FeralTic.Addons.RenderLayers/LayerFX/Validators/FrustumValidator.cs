using FeralTic.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.LayerFX
{
    public class FrustrumValidator : IObjectLayerValidator
    {
        private LayerSettings settings;
        private Frustrum frustrum = new Frustrum();

        public int Passed { get; set; }
        public int Failed { get; set; }
        public bool Enabled { get; set; }

        public void SetGlobalSettings(LayerSettings settings)
        {
            this.settings = settings;
            this.frustrum.Initialize(settings.View, settings.Projection);
        }

        public bool Validate(ObjectLayerSettings obj)
        {
            bool res = this.frustrum.Contains(obj.Geometry.BoundingBox, obj.WorldTransform);
            if (res) { Passed++; } else { Failed++; }
            return res;
        }

        public void Reset()
        {
            this.Passed = 0;
            this.Failed = 0;
        }
    }
}
