using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers
{
    public interface ICustomShaderVariable
    {
        string Name { get; }
        string TypeName { get; }
        string Semantic { get; }
    }
}
