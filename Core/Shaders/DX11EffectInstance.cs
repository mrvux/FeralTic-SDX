using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.D3DCompiler;


namespace FeralTic.DX11
{

    public partial class DX11EffectInstance : IDisposable
    {
        public class CounterResetUAV
        {
            public CounterResetUAV(UnorderedAccessView uav, int counter)
            {
                this.UAV = uav;
                this.Counter = counter;
            }

            public UnorderedAccessView UAV;
            public int Counter;
        }

        private Effect effect;
        private DX11Device device;

        private EffectTechnique currenttechnique;

        private ShaderResourceView[] nullsrvs = new ShaderResourceView[128];
        private UnorderedAccessView[] nulluavs = new UnorderedAccessView[8];

        private List<CounterResetUAV> resetuavs = new List<CounterResetUAV>();

        public Effect Effect { get { return this.effect; } }

        public EffectTechnique CurrentTechnique { get { return this.currenttechnique; } }
        public DX11Device Device { get { return this.device; } }


        public DX11EffectInstance(DX11Device device, DX11Effect effect)
        {
            this.device = device;
            this.effect = new Effect(device.Device, effect.ByteCode);
            this.currenttechnique = this.effect.GetTechniqueByIndex(0);
        }

        public DX11EffectInstance(DX11Device device, ShaderBytecode bytecode)
        {
            this.device = device;
            this.effect = new Effect(device.Device, bytecode);
            this.currenttechnique = this.effect.GetTechniqueByIndex(0);
        }

        public DX11EffectInstance(DX11Device device, Effect effect)
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

        public void ApplyPass(DX11RenderContext context, int index = 0)
        {
            this.currenttechnique.GetPassByIndex(index).Apply(context.Context);
        }

        public void ApplyPass(DX11RenderContext context,EffectPass pass)
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
