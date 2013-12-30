using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace FeralTic.RenderLayers
{
    public interface IViewProjectionSettings
    {
        Matrix View { get; set; }
        Matrix Projection { get; set; }
        Matrix ViewProjection { get; set; }
    }
}
