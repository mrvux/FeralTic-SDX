using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX;

namespace FeralTic.DX11
{
    public class RenderState : IDxResource
    {
        private DxDevice device;

        public RasterizerState Rasterizer { get; set; }

        public DepthStencilState DepthStencil { get; set; }

        public BlendState Blend { get; set; }

        public Color4 BlendFactor { get; set; }

        public int DepthStencilReference { get; set; }

        public int BlendSampleMask { get; set; }

        public RenderState(DxDevice device)
        {
            this.device = device;
            this.BlendFactor = new Color4(0, 0, 0, 0);
            this.DepthStencilReference = 0;
            this.BlendSampleMask = int.MaxValue;
        }

        public RenderState Clone()
        {
            RenderState result = new RenderState(this.device);
            result.Blend = this.Blend;
            result.DepthStencil = this.DepthStencil;
            result.Rasterizer = this.Rasterizer;
            result.DepthStencilReference = this.DepthStencilReference;
            result.BlendSampleMask = this.BlendSampleMask;
            result.BlendFactor = this.BlendFactor;
            return result;
        }


        public void Apply(RenderContext context)
        {
            context.Context.Rasterizer.State = this.Rasterizer;
            context.Context.OutputMerger.DepthStencilState = this.DepthStencil;
            context.Context.OutputMerger.DepthStencilReference = this.DepthStencilReference;
            context.Context.OutputMerger.BlendState = this.Blend;
            context.Context.OutputMerger.BlendFactor = this.BlendFactor;
            context.Context.OutputMerger.BlendSampleMask = this.BlendSampleMask;
        }

        public void Dispose()
        {
            
        }
    }
}
