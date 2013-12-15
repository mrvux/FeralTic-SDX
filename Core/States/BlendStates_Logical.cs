#if DIRECTX11_1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11
{
    public partial class BlendStates : RenderStates<BlendState>
    {
        public BlendState1 LogicalInvert { get; private set; }

        protected void InitializeLogical()
        {
            this.CreateLogicalInvert();
        }

        private void CreateLogicalInvert()
        {
            BlendStateDescription1 bs = new BlendStateDescription1()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };

            for (int i = 0; i < 8; i++)
            {
                bs.RenderTarget[i] = new RenderTargetBlendDescription1()
                {
                    IsBlendEnabled = false,
                    IsLogicOperationEnabled = true,
                    LogicOperation = SharpDX.Direct3D11.LogicOperation.Invert,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All,
                };
            }

            this.LogicalInvert = new BlendState1(this.device.Device, bs);
            this.AddState("LogicalInvert", this.LogicalInvert);
        }

    }
}
#endif
