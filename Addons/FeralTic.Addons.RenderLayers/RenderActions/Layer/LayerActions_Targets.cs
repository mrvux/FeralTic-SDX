using FeralTic.DX11;
using FeralTic.RenderLayers.LayerFX;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.RenderActions
{
    public static partial class LayerActions
    {
        public static void ClearTarget(LayerSettings settings, int index, Color4 color)
        {
            var stackelement = settings.RenderContext.RenderTargetStack.Peek();
            if (index < stackelement.RenderTargets.Length)
            {
                settings.RenderContext.Context.ClearRenderTargetView(stackelement.RenderTargets[index].RenderView, color);
            }
        }

        public static void ClearTargets(LayerSettings settings, Color4 color)
        {
            var stackelement = settings.RenderContext.RenderTargetStack.Peek();
            for (int i = 0; i < stackelement.RenderTargets.Length; i++)
            {
                settings.RenderContext.Context.ClearRenderTargetView(stackelement.RenderTargets[i].RenderView, color);
            }
        }

        public static void ClearDepth(LayerSettings settings, bool depth, float depthValue, bool stencil, byte stencilValue)
        {
            var stackelement = settings.RenderContext.RenderTargetStack.Peek();

            if (stackelement.DepthStencil != null && depth || stencil)
            {
                DepthStencilClearFlags flags = 0;
                flags |= depth ? DepthStencilClearFlags.Depth : flags;
                flags |= stencil ? DepthStencilClearFlags.Stencil : flags;
               
                settings.RenderContext.Context.ClearDepthStencilView(stackelement.DepthStencil.DepthView,flags,depthValue,stencilValue);
            }
        }
    }
}
