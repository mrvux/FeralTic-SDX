using FeralTic.RenderLayers.LayerFX;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.RenderActions
{
    public static class LayerShaderActions
    {
        public static void ApplyTargetSize2D(EffectVectorVariable variable, LayerSettings settings)
        {
            variable.Set(new Vector2(settings.RenderWidth, settings.RenderHeight));
        }

        public static void ApplyInvTargetSize2D(EffectVectorVariable variable, LayerSettings settings)
        {
            variable.Set(new Vector2(1.0f / (float)settings.RenderWidth, 1.0f / (float)settings.RenderHeight));
        }

        public static void ApplySize2D(EffectVectorVariable variable, LayerSettings settings)
        {
            variable.Set(new Vector4(settings.RenderWidth, settings.RenderHeight, 1.0f / (float)settings.RenderWidth, 1.0f / (float)settings.RenderHeight));
        }

        public static void ApplyTargetSize3D(EffectVectorVariable variable, LayerSettings settings)
        {
            variable.Set(new Vector3(settings.RenderWidth, settings.RenderHeight, settings.RenderDepth));
        }

        public static void ApplyInvTargetSize3D(EffectVectorVariable variable, LayerSettings settings)
        {
            variable.Set(new Vector3(1.0f / (float)settings.RenderWidth, 1.0f / (float)settings.RenderHeight, 1.0f / (float)settings.RenderDepth));
        }

        public static void ApplySRVRead(EffectShaderResourceVariable variable, LayerSettings settings)
        {
            variable.SetResource(settings.ReadBuffer.ShaderView);
        }

        public static void ApplyUAVWrite(EffectUnorderedAccessViewVariable variable, LayerSettings settings)
        {
            variable.Set(settings.BackBuffer.UnorderedView);
        }

        public static void ApplyDrawCountInt(EffectScalarVariable variable, LayerSettings settings)
        {
            variable.Set(settings.DrawCallCount);
        }

        public static void ApplyDrawCountFloat(EffectScalarVariable variable, LayerSettings settings)
        {
            variable.Set((float)settings.DrawCallCount);
        }

        public static void ApplyInvDrawCount(EffectScalarVariable variable, LayerSettings settings)
        {
            variable.Set(1.0f / (float)settings.DrawCallCount);
        }

        public static void ApplyViewPortCountInt(EffectScalarVariable variable, LayerSettings settings)
        {
            variable.Set(settings.ViewportCount);
        }

        public static void ApplyViewPortIndexInt(EffectScalarVariable variable, LayerSettings settings)
        {
            variable.Set(settings.ViewportIndex);
        }
    }
}
