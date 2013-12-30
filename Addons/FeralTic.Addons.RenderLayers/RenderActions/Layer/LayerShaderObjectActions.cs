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
    public static class LayerShaderObjectActions
    {
        public static void ApplyWorld(EffectMatrixVariable variable, IViewProjectionSettings settings, ObjectLayerSettings obj)
        {
            variable.SetMatrix(obj.WorldTransform);
        }

        public static void ApplyWorldInverse(EffectMatrixVariable variable, IViewProjectionSettings settings, ObjectLayerSettings obj)
        {
            variable.SetMatrix(Matrix.Invert(obj.WorldTransform));
        }

        public static void ApplyWorldTranspose(EffectMatrixVariable variable, IViewProjectionSettings settings, ObjectLayerSettings obj)
        {
            variable.SetMatrix(Matrix.Transpose(obj.WorldTransform));
        }

        public static void ApplyWorldInverseTranspose(EffectMatrixVariable variable, IViewProjectionSettings settings, ObjectLayerSettings obj)
        {
            variable.SetMatrix(Matrix.Transpose(Matrix.Invert(obj.WorldTransform)));
        }

        public static void ApplyWorldView(EffectMatrixVariable variable, IViewProjectionSettings settings, ObjectLayerSettings obj)
        {
            variable.SetMatrix(obj.WorldTransform * settings.View);
        }

        public static void ApplyWorldViewProjection(EffectMatrixVariable variable, IViewProjectionSettings settings, ObjectLayerSettings obj)
        {
            variable.SetMatrix(obj.WorldTransform * settings.ViewProjection);
        }


        public static void ApplyDrawIndexInt(EffectScalarVariable variable, LayerSettings settings, ObjectLayerSettings obj)
        {
            variable.Set(obj.DrawCallIndex);
        }

        public static void ApplyDrawIndexFloat(EffectScalarVariable variable, LayerSettings settings, ObjectLayerSettings obj)
        {
            variable.Set((float)obj.DrawCallIndex);
        }

        public static void ApplyBoundingMin(EffectVectorVariable variable, LayerSettings settings, ObjectLayerSettings obj)
        {
            if (obj.Geometry != null)
            {
                if (obj.Geometry.HasBoundingBox)
                {
                    variable.Set(obj.Geometry.BoundingBox.Minimum);
                    return;
                }
            }
            variable.Set(new Vector3(-0.5f, -0.5f, -0.5f));
        }

        public static void ApplyBoundingMax(EffectVectorVariable variable, LayerSettings settings, ObjectLayerSettings obj)
        {
            if (obj.Geometry != null)
            {
                if (obj.Geometry.HasBoundingBox)
                {
                    variable.Set(obj.Geometry.BoundingBox.Maximum);
                    return;
                }
            }
            variable.Set(new Vector3(0.5f, 0.5f, 0.5f));
        }

        public static void ApplyBoundingScale(EffectVectorVariable variable, LayerSettings settings, ObjectLayerSettings obj)
        {
            if (obj.Geometry != null)
            {
                if (obj.Geometry.HasBoundingBox)
                {
                    variable.Set(obj.Geometry.BoundingBox.Maximum - obj.Geometry.BoundingBox.Minimum);
                    return;
                }
            }
            variable.Set(new Vector3(1, 1, 1));
        }

        public static void ApplyUnitTransform(EffectMatrixVariable variable, LayerSettings settings, ObjectLayerSettings obj)
        {
            if (obj.Geometry != null)
            {
                if (obj.Geometry.HasBoundingBox)
                {
                    Vector3 scale = obj.Geometry.BoundingBox.Maximum - obj.Geometry.BoundingBox.Minimum;
                    scale.X = scale.X != 0.0f ? 1.0f / scale.X : 1.0f;
                    scale.Y = scale.Y != 0.0f ? 1.0f / scale.Y : 1.0f;
                    scale.Z = scale.Z != 0.0f ? 1.0f / scale.Z : 1.0f;
                    Matrix m = Matrix.Scaling(scale);
                    variable.SetMatrix(m);
                    return;
                }
            }
            variable.SetMatrix(Matrix.Identity);
        }

        public static void ApplySDFTransform(EffectMatrixVariable variable, LayerSettings settings, ObjectLayerSettings obj)
        {
            if (obj.Geometry != null)
            {
                if (obj.Geometry.HasBoundingBox)
                {
                    Vector3 min = obj.Geometry.BoundingBox.Minimum;
                    Vector3 scale = obj.Geometry.BoundingBox.Maximum - obj.Geometry.BoundingBox.Minimum;
                    scale.X = scale.X != 0.0f ? scale.X : 1.0f;
                    scale.Y = scale.Y != 0.0f ? scale.Y : 1.0f;
                    scale.Z = scale.Z != 0.0f ? scale.Z : 1.0f;
                    Matrix m = Matrix.Scaling(scale);
                    m.M41 = min.X;
                    m.M42 = min.Y;
                    m.M43 = min.Z;
                    variable.SetMatrix(Matrix.Invert(m));
                    return;
                }
            }
            variable.SetMatrix(Matrix.Identity);
        }
    }
}
