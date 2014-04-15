using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D11;

namespace FeralTic.DX11
{
    public partial class EffectInstance
    {
        #region Scalar
        public void SetBySemantic(string name, bool value)
        {
            this.effect.GetVariableBySemantic(name).AsScalar().Set(value);
        }

        public void SetBySemantic(string name, int value)
        {
            this.effect.GetVariableBySemantic(name).AsScalar().Set(value);
        }

        public void SetBySemantic(string name, float value)
        {
            this.effect.GetVariableBySemantic(name).AsScalar().Set(value);
        }

        public void SetBySemantic(string name, bool[] value)
        {
            this.effect.GetVariableBySemantic(name).AsScalar().Set(value);
        }

        public void SetBySemantic(string name, int[] value)
        {
            this.effect.GetVariableBySemantic(name).AsScalar().Set(value);
        }

        public void SetBySemantic(string name, float[] value)
        {
            this.effect.GetVariableBySemantic(name).AsScalar().Set(value);
        }
        #endregion

        #region Vectors
        public void SetBySemantic(string name, Vector2 value)
        {
            this.effect.GetVariableBySemantic(name).AsVector().Set(value);
        }

        public void SetBySemantic(string name, Vector3 value)
        {
            this.effect.GetVariableBySemantic(name).AsVector().Set(value);
        }

        public void SetBySemantic(string name, Vector4 value)
        {
            this.effect.GetVariableBySemantic(name).AsVector().Set(value);
        }

        public void SetBySemantic(string name, Color4 value)
        {
            this.effect.GetVariableBySemantic(name).AsVector().Set(value);
        }

        public void SetBySemantic(string name, Vector4[] value)
        {
            this.effect.GetVariableBySemantic(name).AsVector().Set(value);
        }

        public void SetBySemantic(string name, Color4[] value)
        {
            this.effect.GetVariableBySemantic(name).AsVector().Set(value);
        }
        #endregion

        #region Transforms
        public void SetBySemantic(string name, Matrix value)
        {
            this.effect.GetVariableBySemantic(name).AsMatrix().SetMatrix(value);
        }

        /*public void SetBySemantic(string name, Matrix[] value)
        {
            this.effect.GetVariableBySemantic(name).AsMatrix().se(value);
        }*/
        #endregion

        #region Resources
        public void SetBySemantic(string name, ShaderResourceView value)
        {
            this.effect.GetVariableBySemantic(name).AsShaderResource().SetResource(value);
        }

        public void SetBySemantic(string name, UnorderedAccessView value)
        {
            this.effect.GetVariableBySemantic(name).AsUnorderedAccessView().Set(value);
        }

        public void SetBySemantic(string name, UnorderedAccessView value, int counter = -1)
        {
            this.effect.GetVariableBySemantic(name).AsUnorderedAccessView().Set(value);
            this.resetuavs.Add(new CounterResetUAV(value, counter));
        }
        #endregion
    }
}
