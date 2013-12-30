using FeralTic.DX11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers
{
    public interface IShaderSemantic : IDxResource
    {
        string TypeName { get; }
        string Semantic { get; }
        bool Mandatory { get; }
        bool Apply(EffectInstance instance, List<ICustomShaderVariable> variables);
    }
}
