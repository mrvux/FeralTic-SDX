using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11
{
    public partial class BlendStates : RenderStates<BlendState>
    {
        private DxDevice device;
         
        public BlendState Disabled { get; private set; }
        public BlendState Additive { get; private set; }
        public BlendState AlphaBlend { get; private set; }
        public BlendState Multiply { get; private set; }
        public BlendState AlphaAdd { get; private set; }
        public BlendState DisabledAlphaWrite { get; private set; }
        public BlendState DisabledColorWrite { get; private set; }

        public BlendStates(DxDevice device)
        {
            this.device = device;
            this.Initialize();
            #if DIRECTX11_1
            this.InitializeLogical();
            #endif
        }

        public override string EnumName
        {
            get
            {
                return "DX11.BlendPresets";
            }
        }

        protected override void Initialize()
        {
            this.CreateNoBlend();
            this.CreateAddivite();
            this.CreateBlend();
            this.CreateMultiply();
            this.CreateAlphaAdd();
        }

        private void CreateNoBlend()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTarget[i] = new RenderTargetBlendDescription()
                {
                    IsBlendEnabled = false,
                    BlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.InverseSourceAlpha,
                    DestinationAlphaBlend = BlendOption.One,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.SourceAlpha,
                    SourceAlphaBlend = BlendOption.One
                };
            }

            this.Disabled = new BlendState(this.device.Device, bs);
            this.AddState("Disabled",this.Disabled);

            bs.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.Alpha;
            this.DisabledAlphaWrite = new BlendState(this.device, bs);

            bs.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Blue | ColorWriteMaskFlags.Green;
            this.DisabledColorWrite = new BlendState(this.device, bs);

        }

        private void CreateAddivite()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                //bs.IndependentBlendEnable 

                bs.RenderTarget[i] = new RenderTargetBlendDescription()
                {
                    IsBlendEnabled = true,
                    BlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.One,
                    DestinationAlphaBlend = BlendOption.One,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.One,
                    SourceAlphaBlend = BlendOption.One
                };
            }

            this.Additive = new BlendState(this.device.Device, bs);
            this.AddState("Add", this.Additive);
        }

        private void CreateBlend()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTarget[i] = new RenderTargetBlendDescription()
                {
                    IsBlendEnabled = true,
                    BlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.InverseSourceAlpha,
                    DestinationAlphaBlend = BlendOption.One,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.SourceAlpha,
                    SourceAlphaBlend = BlendOption.One
                };
            }

            this.AlphaBlend = new BlendState(this.device.Device, bs);

            this.AddState("Blend", this.AlphaBlend);
        }

        private void CreateMultiply()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTarget[i] = new RenderTargetBlendDescription()
                {
                    IsBlendEnabled = true,
                    BlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.Zero,
                    DestinationAlphaBlend = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.DestinationColor,
                    SourceAlphaBlend = BlendOption.DestinationAlpha
                };
            }

            this.Multiply = new BlendState(this.device.Device, bs);
            this.AddState("Multiply", this.Multiply);
        }

        private void CreateAlphaAdd()
        {
            BlendStateDescription bs = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            for (int i = 0; i < 8; i++)
            {
                bs.RenderTarget[i] = new RenderTargetBlendDescription()
                {
                    IsBlendEnabled = true,
                    BlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                    DestinationBlend = BlendOption.One,
                    DestinationAlphaBlend = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    SourceBlend = BlendOption.SourceAlpha,
                    SourceAlphaBlend = BlendOption.Zero
                };
            }

            this.AlphaAdd = new BlendState(this.device.Device, bs);
            this.AddState("AlphaAdd", this.AlphaAdd);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.DisabledColorWrite.Dispose();
            this.DisabledAlphaWrite.Dispose();
        }

    }
}
