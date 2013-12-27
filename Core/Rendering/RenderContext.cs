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
    public class RenderContext : IDisposable
    {
        public DxDevice Device { get; protected set; }

        public DirectXContext Context { get; protected set; }

        private static ShaderResourceView[] nullsrvs = new ShaderResourceView[128];
        private static UnorderedAccessView[] nulluavs = new UnorderedAccessView[8];

        public RenderTargetStack RenderTargetStack { get; protected set; }
        public RenderStateStack RenderStateStack { get; protected set; }

        public static implicit operator DirectXContext(RenderContext context)
        {
            return context.Context;
        }

        protected RenderContext()
        {

        }

        public RenderContext(DxDevice device)
        {
            this.Device = device;

            #if DIRECTX11_1
            this.Context = device.Device.ImmediateContext.QueryInterface<DirectXContext>();
            #else
            this.Context = device.Device.ImmediateContext;
            #endif

            this.RenderTargetStack = new RenderTargetStack(this);
            this.RenderStateStack = new RenderStateStack(this);
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
