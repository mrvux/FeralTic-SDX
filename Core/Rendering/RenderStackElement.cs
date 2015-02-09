using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX;

namespace FeralTic.DX11
{
    public class RenderTargetStackElement
    {
        public IDxRenderTarget[] RenderTargets { get; private set; }

        private RenderTargetView[] rtvs;

        public IDxDepthStencil DepthStencil { get; private set; }

        private ViewportF vp;
        private Rectangle scissor;
        private bool hasscissor;
        private bool rodsv = false;

        public RenderTargetStackElement(ViewportF vp, Rectangle scissor, IDxDepthStencil dsv, bool rodsv = false, params IDxRenderTarget[] rts)
        {
            this.DepthStencil = dsv;
            this.RenderTargets = rts;
            this.rodsv = rodsv;

            this.vp = vp;
            this.scissor = scissor;
            this.hasscissor = true;

            rtvs = new RenderTargetView[rts.Length];
            for (int i = 0; i < rts.Length; i++) { rtvs[i] = rts[i].RenderView; }
        }

        public RenderTargetStackElement(ViewportF vp, IDxDepthStencil dsv, bool rodsv = false, params IDxRenderTarget[] rts)
        {
            this.DepthStencil = dsv;
            this.RenderTargets = rts;
            this.rodsv = rodsv;

            this.vp = vp;
            this.hasscissor = false;

            rtvs = new RenderTargetView[rts.Length];
            for (int i = 0; i < rts.Length; i++) { rtvs[i] = rts[i].RenderView; }
        }

        public RenderTargetStackElement(IDxDepthStencil dsv, bool rodsv = false, params IDxRenderTarget[] rts)
        {
            this.DepthStencil = dsv;
            this.RenderTargets = rts;
            this.rodsv = rodsv;

            this.vp.X = 0;
            this.vp.Y = 0;
            this.vp.MinDepth = 0.0f;
            this.vp.MaxDepth = 1.0f;

            this.hasscissor = false;

            if (this.DepthStencil != null)
            {
                this.vp.Width = this.DepthStencil.Width;
                this.vp.Height = this.DepthStencil.Height;
            }
            else
            {
                this.vp.Width = rts[0].Width;
                this.vp.Height = rts[0].Height;
            }

            rtvs = new RenderTargetView[rts.Length];
            for (int i = 0; i < rts.Length; i++) { rtvs[i] = rts[i].RenderView; }
        }

        public void Apply(DeviceContext ctx)
        {
            if (DepthStencil != null)
            {
                if (rodsv)
                {
                    ctx.OutputMerger.SetTargets(DepthStencil.ReadOnlyView, this.rtvs);
                }
                else
                {
                    ctx.OutputMerger.SetTargets(DepthStencil.DepthView, this.rtvs);
                }
            }
            else
            {
                ctx.OutputMerger.SetTargets(this.rtvs);
            }
            ctx.Rasterizer.SetViewport(vp);
            if (this.hasscissor) { ctx.Rasterizer.SetScissorRectangles(scissor); }
            else { ctx.Rasterizer.SetScissorRectangles(null); }
        }

    }
}
