using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct3D11;

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


            SharpDX.Direct3D11.Device d = new Device(SharpDX.Direct3D.DriverType.Hardware,DeviceCreationFlags.VideoSupport | DeviceCreationFlags.BgraSupport);
            

           /* var device = new RenderDevice(, 1);
            RenderContext context = new RenderContext(device);

            var d = device.Device;*/
            var multithread = d.QueryInterface<SharpDX.Direct3D.DeviceMultithread>();
            multithread.SetMultithreadProtected(true);

            // Create a DXGI Device Manager
            var dxgiDeviceManager = new SharpDX.MediaFoundation.DXGIDeviceManager();
            dxgiDeviceManager.ResetDevice(d);

            VideoDevice vd = d.QueryInterface<VideoDevice>();

            //vd.VideoDecoderProfileCount

            /*VideoDecoderDescription desc = new VideoDecoderDescription()
            {
                
            }*/

            VideoContext ctx = d.ImmediateContext.QueryInterface<VideoContext>();

            /*var swapChain = new DX11SwapChain(device, renderForm.Handle);
            
            /*var dx = new VideoDecoderDescription()
            {
                
            }*/
            /*int i = vd.VideoDecoderProfileCount;

            renderForm.ResizeEnd += (s, e) => swapChain.Resize();

            RenderLoop.Run(renderForm, () =>
            {
                context.Context.ClearRenderTargetView(swapChain.RenderView, new SharpDX.Color4(1, 1, 1, 1));

                swapChain.Present(1, SharpDX.DXGI.PresentFlags.None);
            });*/

        }

        private static void ResizeEnd(object sender, EventArgs e)
        {
            
        }
    }
}
