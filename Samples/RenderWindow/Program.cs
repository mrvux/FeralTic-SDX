using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenderWindow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var renderForm = new RenderForm("");
            

            var device = new RenderDevice();
            var swapChain = new DX11SwapChain(device, renderForm.Handle);
            RenderContext context = new RenderContext(device);

            renderForm.ResizeEnd += (s, e) => swapChain.Resize();

            RenderLoop.Run(renderForm, () =>
            {
                context.Context.ClearRenderTargetView(swapChain.RenderView, new SharpDX.Color4(1, 1, 1, 1));

                swapChain.Present(1, SharpDX.DXGI.PresentFlags.None);
            });

        }

        private static void ResizeEnd(object sender, EventArgs e)
        {
            
        }
    }
}
