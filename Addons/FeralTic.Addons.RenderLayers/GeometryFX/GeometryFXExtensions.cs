using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.GeometryFX
{
    public static class GeometryFXExtentions
    {
        public static InputElement[] GetStreamOutputLayout(this EffectPass pass, out int vertexsize)
        {
            vertexsize = 0;
            if (pass.GeometryShaderDescription.Variable == null)
            {
                return new InputElement[0];
            }
            else
            {
                EffectShaderVariable gs = pass.GeometryShaderDescription.Variable;
                int outputcount = gs.GetShaderDescription(0).OutputParameterCount;

                InputElement[] elems = new InputElement[outputcount];

                int offset = 0;

                for (int vip = 0; vip < outputcount; vip++)
                {
                    ShaderParameterDescription sd = gs.GetOutputSignatureElementDescription(0, vip);
                    int componentcount = 0;

                    if (sd.UsageMask.HasFlag(RegisterComponentMaskFlags.ComponentX)) { componentcount++; }
                    if (sd.UsageMask.HasFlag(RegisterComponentMaskFlags.ComponentY)) { componentcount++; }
                    if (sd.UsageMask.HasFlag(RegisterComponentMaskFlags.ComponentZ)) { componentcount++; }
                    if (sd.UsageMask.HasFlag(RegisterComponentMaskFlags.ComponentW)) { componentcount++; }

                    int vsize = 4 * componentcount;

                    string fmt = "";
                    if (componentcount == 1) { fmt = "R32_"; }
                    if (componentcount == 2) { fmt = "R32G32_"; }
                    if (componentcount == 3) { fmt = "R32G32B32_"; }
                    if (componentcount == 4) { fmt = "R32G32B32A32_"; }

                    switch (sd.ComponentType)
                    {
                        case RegisterComponentType.Float32:
                            fmt += "Float";
                            break;
                        case RegisterComponentType.SInt32:
                            fmt += "SInt";
                            break;
                        case RegisterComponentType.UInt32:
                            fmt += "UInt";
                            break;
                    }

                    Format f = (Format)Enum.Parse(typeof(Format), fmt);
                    InputElement elem = new InputElement(sd.SemanticName, (int)sd.SemanticIndex, f, offset, 0);
                    elems[vip] = elem;
                    offset += vsize;
                    vertexsize += vsize;
                }

                return elems;
            }
        }
    }
}
