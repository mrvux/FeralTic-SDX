using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if DIRECTX11_2
using DirectXContext = SharpDX.Direct3D11.DeviceContext2;
#else
#if DIRECTX11_1
using DirectXContext = SharpDX.Direct3D11.DeviceContext1;
#else
using DirectXContext = SharpDX.Direct3D11.DeviceContext;
#endif
#endif

namespace FeralTic.DX11
{
    public class DefferedRenderContext : RenderContext
    {
        public DefferedRenderContext(DxDevice device)
        {
            this.Device = device;
            this.Context = new DirectXContext(device.Device);
            this.RenderTargetStack = new RenderTargetStack(this);
            this.RenderStateStack = new RenderStateStack(this);
        }

        public CommandList CommandList { get; private set; }

        public void Finish(bool restore = false)
        {
            this.CommandList = this.Context.FinishCommandList(restore);
        }

        public void Execute(RenderContext context, bool restore = false)
        {
            context.Context.ExecuteCommandList(this.CommandList, restore);
            this.CommandList.Dispose();
        }
    }
}
