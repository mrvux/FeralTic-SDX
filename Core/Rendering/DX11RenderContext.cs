using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public class DX11RenderContext : IDisposable
    {
        public DxDevice Device { get; protected set; }

        public DeviceContext2 Context { get; protected set; }

        private static ShaderResourceView[] nullsrvs = new ShaderResourceView[128];
        private static UnorderedAccessView[] nulluavs = new UnorderedAccessView[8];

        public DX11RenderTargetStack RenderTargetStack { get; protected set; }
        public DX11RenderStateStack RenderStateStack { get; protected set; }

        public static implicit operator DeviceContext2(DX11RenderContext context)
        {
            return context.Context;
        }

        protected DX11RenderContext()
        {

        }

        public DX11RenderContext(DxDevice device)
        {
            this.Device = device;
            this.Context = device.Device.ImmediateContext.QueryInterface<DeviceContext2>();
            this.RenderTargetStack = new DX11RenderTargetStack(this);
            this.RenderStateStack = new DX11RenderStateStack(this);
            this.RenderStateStack.PushDefault();
        }

        public void ClearShaderStages()
        {
            Context.VertexShader.Set(null);
            Context.HullShader.Set(null);
            Context.DomainShader.Set(null);
            Context.GeometryShader.Set(null);
            Context.PixelShader.Set(null);
            Context.ComputeShader.Set(null);
        }

        public void CleanUpPS()
        {
            Context.PixelShader.SetShaderResources(0, nullsrvs);
        }

        public void CleanUpCS()
        {
            Context.ComputeShader.SetShaderResources(0, nullsrvs);
            Context.ComputeShader.SetUnorderedAccessViews(0, nulluavs);
        }

        public void Dispose()
        {
            if (this.Context != null) { this.Context.Dispose(); this.Context = null; }
        }
    }
}
