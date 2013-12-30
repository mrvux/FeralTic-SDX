using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.TextureFX
{
    public static class TextureFXActions
    {
        public static void ApplyTargetSize(EffectVectorVariable variable, TextureFXSettings settings)
        {
            variable.Set(new Vector2(settings.CurrentTarget.Width, settings.CurrentTarget.Height));
        }

        public static void ApplyInvTargetSize(EffectVectorVariable variable, TextureFXSettings settings)
        {
            variable.Set(new Vector2(1.0f / (float)settings.CurrentTarget.Width, 1.0f / (float)settings.CurrentTarget.Height));
        }

        public static void ApplyFullTargetSize(EffectVectorVariable variable, TextureFXSettings settings)
        {
            variable.Set(new Vector4(settings.CurrentTarget.Width, settings.CurrentTarget.Height, 1.0f / (float)settings.CurrentTarget.Width, 1.0f / (float)settings.CurrentTarget.Height));
        }

        public static void ApplyInitialTexture(EffectShaderResourceVariable variable, TextureFXSettings settings)
        {
            if (settings.InitialTexture == null)
            {
                variable.SetResource(null);
            }
            else
            {
                variable.SetResource(settings.InitialTexture.ShaderView);
            }
        }

        public static void ApplyPreviousTexture(EffectShaderResourceVariable variable, TextureFXSettings settings)
        {
            if (settings.PreviousTexture == null)
            {
                variable.SetResource(null);
            }
            else
            {
                variable.SetResource(settings.PreviousTexture.ShaderView);
            }
        }

        public static void ApplyPassIndex(EffectScalarVariable variable, TextureFXSettings settings, TextureFXPassSettings passsettings)
        {
            variable.Set(passsettings.PassIndex);
        }
    }
}
