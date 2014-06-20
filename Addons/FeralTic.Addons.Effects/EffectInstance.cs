using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.D3DCompiler;


namespace FeralTic.DX11
{

    public partial class EffectInstance : IDisposable
    {
        private Effect effect;
        private DxDevice device;

        private EffectTechnique currenttechnique;

        public Effect Effect { get { return this.effect; } }

        public EffectTechnique CurrentTechnique { get { return this.currenttechnique; } }
        public DxDevice Device { get { return this.device; } }

        public EffectInstance(DxDevice device, DX11Effect effect)
        {
            this.device = device;
            this.effect = new Effect(device.Device, effect.ByteCode);
            this.currenttechnique = this.effect.GetTechniqueByIndex(0);
        }

        public EffectInstance(DxDevice device, ShaderBytecode bytecode)
        {
            this.device = device;
            this.effect = new Effect(device.Device, bytecode);
            this.currenttechnique = this.effect.GetTechniqueByIndex(0);
        }

        public EffectInstance(DxDevice device, Effect effect)
        {
            this.device = device;
            this.effect = effect;
            this.currenttechnique = this.effect.GetTechniqueByIndex(0);
        }

        public bool HasTechnique(string name)
        {
            return this.effect.GetTechniqueByName(name).IsValid;
        }

        public void SelectTechnique(string name)
        {
            this.currenttechnique = this.effect.GetTechniqueByName(name);
        }

        public void SelectTechnique(int index)
        {
            this.currenttechnique = this.effect.GetTechniqueByIndex(index);
        }

        public void ApplyPass(RenderContext context, int index = 0)
        {
            this.currenttechnique.GetPassByIndex(index).Apply(context.Context,0);
        }

        public void ApplyPass(RenderContext context,EffectPass pass)
        {
            pass.Apply(context.Context);
        }

        public EffectPass GetPass(int index)
        {
            return this.currenttechnique.GetPassByIndex(index);
       } 

        public void Dispose()
        {
            this.effect.Dispose();
        }

        public void BindClass(string classname, string interfacename)
        {
            this.Effect.GetVariableByName(interfacename).AsInterface().ClassInstance = this.Effect.GetVariableByName(classname).AsClassInstance();
        }
    }
}
