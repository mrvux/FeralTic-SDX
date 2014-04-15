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
        private DxDevice device;

        private EffectTechnique currenttechnique;

        private ShaderResourceView[] nullsrvs = new ShaderResourceView[128];
        private UnorderedAccessView[] nulluavs = new UnorderedAccessView[8];

        private List<CounterResetUAV> resetuavs = new List<CounterResetUAV>();

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
            this.currenttechnique.GetPassByIndex(index).Apply(context.Context);
            this.ApplyCounterUAVS(context);
        }

        public void ApplyPass(RenderContext context,EffectPass pass)
        {
            pass.Apply(context.Context);
            this.ApplyCounterUAVS(context);
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

        #region Apply Counter UAVS
        private void ApplyCounterUAVS(RenderContext context)
        {
            foreach (CounterResetUAV ru in this.resetuavs)
            {

                int i = 0;
                bool found = false;

                //Get currently bound UAVs
                UnorderedAccessView[] uavs = context.Context.ComputeShader.GetUnorderedAccessViews(0, 8);

                //Search for uav slot, if found, reapply with counter value
                for (i = 0; i < 8 && !found; i++)
                {
                    if (uavs[i].NativePointer == ru.UAV.NativePointer)
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    context.Context.ComputeShader.SetUnorderedAccessView(i - 1, ru.UAV, ru.Counter);
                }
            }
            this.resetuavs.Clear();
        }
        #endregion

    }
}
