using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX;

namespace FeralTic.DX11
{
    public class RenderTargetStack
    {
        private RenderContext context;

        private Stack<RenderTargetStackElement> stack;

        private ViewportStack viewportstack;

        public int StackCount { get { return this.stack.Count; } }
        public int ViewPortStackCount { get { return this.viewportstack.Count; } }

        public RenderTargetStack(RenderContext context)
        {
            this.context = context;
            this.viewportstack = new ViewportStack(context);

            stack = new Stack<RenderTargetStackElement>();
        }

        public void Push(ViewportF vp, IDxDepthStencil dsv, bool rodsv = false, params IDxRenderTarget[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(vp, dsv, rodsv, rts);
            stack.Push(elem);
            this.Apply();
        }

        public void Push(RenderTargetStackElement element)
        {
            stack.Push(element);
            this.Apply();
        }

        public void Push(ViewportF vp, Rectangle scissor, IDxDepthStencil dsv, bool rodsv = false, params IDxRenderTarget[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(vp,scissor, dsv, rodsv, rts);
            stack.Push(elem);
            this.Apply();
        }

        public void Push(IDxDepthStencil dsv, bool rodsv = false, params IDxRenderTarget[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(dsv, rodsv, rts);
            stack.Push(elem);
            this.Apply();
        }

        public void Push(params IDxRenderTarget[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(null, false, rts);
            stack.Push(elem);
            this.Apply();
        }

        public void PushViewport(Viewport vp)
        {
            this.viewportstack.Push(vp);
        }

        public void PushViewPort(Viewport vp, Rectangle scissor)
        {
            this.viewportstack.Push(vp, scissor);
        }

        public void PopViewport()
        {
            this.viewportstack.Pop();
        }

        
        public void Pop()
        {
            stack.Pop();
            this.Apply();
        }

        public RenderTargetStackElement Peek()
        {
            return stack.Peek();
        }

        public void Apply()
        {
            if (stack.Count > 0)
            {
                stack.Peek().Apply(context.Context);
            }
            else
            {
                RenderTargetView[] zero = new RenderTargetView[] { null,null,null,null,null,null,null,null };
                context.Context.OutputMerger.SetTargets(null, zero);
            }
        }


    }
}
