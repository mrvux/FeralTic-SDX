using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX;

namespace FeralTic.DX11
{
    public class ViewPortData
    {
        public ViewPortData(ViewportF vp)
        {
            this.Viewport = vp;
            this.HasScissor = false;
        }

        public ViewPortData(ViewportF vp, Rectangle scissor)
        {
            this.Viewport = vp;
            this.HasScissor = true;
            this.Scissor = scissor;
        }

        public ViewportF Viewport { get; protected set; }
        public Rectangle Scissor { get; protected set; }
        public bool HasScissor { get; protected set;}

        public void Apply(DX11RenderContext context)
        {
            context.Context.Rasterizer.SetViewport(this.Viewport);

            if (this.HasScissor)
            {
                context.Context.Rasterizer.SetScissorRectangles(this.Scissor);
            }
            else
            {
                context.Context.Rasterizer.SetScissorRectangles(null);
            }
        }

    }

    public class ViewportStack
    {
        private DX11RenderContext context;

        private Stack<ViewPortData> stack;

        public int Count
        {
            get { return stack.Count; }
        }

        public ViewportStack(DX11RenderContext context)
        {
            this.context = context;
            stack = new Stack<ViewPortData>();
        }

        public void Push(ViewportF vp)
        {
            stack.Push(new ViewPortData(vp));
            this.Apply();
        }

        public void Push(ViewportF vp, Rectangle scissor)
        {
            stack.Push(new ViewPortData(vp,scissor));
            this.Apply();
        }


        public void Pop()
        {
            stack.Pop();
            this.Apply();
        }

        public void Apply()
        {
            if (stack.Count > 0)
            {
                stack.Peek().Apply(context);
            }
        }
    }
}
