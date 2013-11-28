using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public class RenderStateStack
    {
        private RenderContext context;

        private RenderState defaultstate;

        private Stack<RenderState> stack;

        public int Count
        {
            get { return this.stack.Count; }
        }

        public RenderStateStack(RenderContext context)
        {
            this.context = context;
            this.defaultstate = new RenderState(context.Device);
            stack = new Stack<RenderState>();
        }

        public void PushDefault()
        {
            this.Push(this.defaultstate);
        }

        public RenderState Peek()
        {
            return stack.Peek();
        }

        public void Push(RenderState state)
        { 
            stack.Push(state);
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
            else
            {
                defaultstate.Apply(context);
            }
        }
    }
}
