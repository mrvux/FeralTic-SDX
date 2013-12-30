using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.RenderActions
{
    public static class ViewProjectionShaderActions
    {
        public static void ApplyProjection(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(settings.Projection);
        }

        public static void ApplyProjectionInverse(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Invert(settings.Projection));
        }

        public static void ApplyProjectionTranspose(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Transpose(settings.Projection));
        }

        public static void ApplyProjectionInverseTranspose(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Transpose(Matrix.Invert(settings.Projection)));
        }


        public static void ApplyView(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(settings.View);
        }

        public static void ApplyViewInverse(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Invert(settings.View));
        }

        public static void ApplyViewTranspose(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Transpose(settings.View));
        }

        public static void ApplyViewInverseTranspose(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Transpose(Matrix.Invert(settings.View)));
        }

        public static void ApplyViewProjection(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(settings.ViewProjection);
        }

        public static void ApplyViewProjectionInverse(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Invert(settings.ViewProjection));
        }

        public static void ApplyViewProjectionTranspose(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Transpose(settings.ViewProjection));
        }

        public static void ApplyViewProjectionInverseTranspose(EffectMatrixVariable variable, IViewProjectionSettings settings)
        {
            variable.SetMatrix(Matrix.Transpose(Matrix.Invert(settings.ViewProjection)));
        }
    }
}
