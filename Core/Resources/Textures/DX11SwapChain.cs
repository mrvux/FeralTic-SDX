using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;

using Device = SharpDX.Direct3D11.Device;
using System.Runtime.InteropServices;

namespace FeralTic.DX11.Resources
{
    public class DX11SwapChain : IDX11RenderTarget
    {
        private DxDevice device;
        private IntPtr handle;
        private SwapChain swapchain;

        public RenderTargetView RenderView { get; protected set; }
        public RenderTargetViewDescription RenderViewDesc { get; protected set; }

        public Texture2DDescription TextureDesc { get; protected set; }
        private Texture2D resource;


        public IntPtr Handle { get { return this.handle; } }

        public int Width
        {
            get { return this.TextureDesc.Width; }
        }

        public int Height
        {
            get { return this.TextureDesc.Height; }
        }

        public DX11SwapChain(DxDevice device, IntPtr handle)
            : this(device, handle, Format.R8G8B8A8_UNorm, new SampleDescription(1,0))
        {

        }
        

        public DX11SwapChain(DxDevice device, IntPtr handle, Format format, SampleDescription sampledesc)
        {
            this.device = device;
            this.handle = handle;

            SwapChainDescription sd = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), format),
                IsWindowed = true,
                OutputHandle = handle,
                SampleDescription = sampledesc,
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput | Usage.ShaderInput,
                Flags = SwapChainFlags.None
            };

            this.swapchain = new SwapChain(device.Factory, device.Device, sd);

            this.resource = Texture2D.FromSwapChain<Texture2D>(this.swapchain, 0);
            this.TextureDesc = this.resource.Description;

            this.RenderView = new RenderTargetView(device.Device, this.resource);
            this.RenderViewDesc = this.RenderView.Description;

        }

        public void Resize()
        {
            this.Resize(0, 0);
        }

        public void Resize(int w, int h)
        {
            

            if (this.RenderView != null) { this.RenderView.Dispose(); }
            this.resource.Dispose();

            this.swapchain.ResizeBuffers(1,w, h, SharpDX.DXGI.Format.Unknown, SwapChainFlags.AllowModeSwitch);

            this.resource = Texture2D.FromSwapChain<Texture2D>(this.swapchain, 0);

            this.TextureDesc = this.resource.Description;
            this.RenderView = new RenderTargetView(device.Device, this.resource);
        }

        public void Present(int syncInterval,PresentFlags flags)
        {
            try
            {
                this.swapchain.Present(syncInterval, flags);
            }
            catch (SharpDXException exception)
            {
                if (exception.ResultCode == SharpDX.DXGI.ResultCode.DeviceRemoved
                    || exception.ResultCode == SharpDX.DXGI.ResultCode.DeviceReset)
                {
                    this.device.NotifyDeviceLost();
                }
                else
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            if (this.RenderView != null) { this.RenderView.Dispose(); }
            if (this.resource != null) { this.resource.Dispose(); }
            if (this.swapchain != null) { this.swapchain.Dispose(); }
        }
    }
}
