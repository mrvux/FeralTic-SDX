using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Shaders
{
    /// <summary>
    /// Defines different shader stages
    /// </summary>
    public enum ShaderStage
    {
        Vertex,
        Hull,
        Domain,
        Geometry,
        Pixel,
        Compute
    }
}
