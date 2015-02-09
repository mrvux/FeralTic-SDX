using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;

using Device = SharpDX.Direct3D11.Device;
using System.Runtime.InteropServices;

namespace FeralTic.DX11.Resources
{
    public class DX11SwapChain : IDxRenderTarget , IDxTexture2D, IDxUnorderedResource
    {
        private DxDevice device;
        private SwapChain swapchain;

        public RenderTargetView RenderView { get; protected set; }
        public RenderTargetViewDescription RenderViewDesc { get; protected set; }

        public Texture2DDescription TextureDesc { get; protected set; }
        private Texture2D resource;

        public UnorderedAccessView UnorderedView { get; protected set; }

        public int Width
        {
            get { return this.TextureDesc.Width; }
        }

        public int Height
        {
            get { return this.TextureDesc.Height; }
        }

        public Texture2D Texture
        {
            get { return this.resource; }
        }

        public Format Format
        {
            get { return this.TextureDesc.Format; }
        }

        public ShaderResourceView ShaderView
        {
            get { return null; }
        }

        public static DX11SwapChain FromHandle(DxDevice device, IntPtr handle)
        {
            return FromHandle(device, handle, Format.R8G8B8A8_UNorm, new SampleDescription(1, 0));
        }
        
        public static DX11SwapChain FromHandle(DxDevice device, IntPtr handle, Format format, SampleDescription sampledesc)
        {
            DX11SwapChain swapShain = new DX11SwapChain();
            swapShain.device = device;

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

            if (device.IsFeatureLevel11 && sampledesc.Count == 1)
            {
                sd.Usage |= Usage.UnorderedAccess;
            }
            swapShain.swapchain = new SwapChain(device.Factory, device.Device, sd);

            swapShain.Initialize();
            return swapShain;
        }

        public static DX11SwapChain FromComposition(DxDevice dxDevice, int w, int h)
        {
            DX11SwapChain swapShain = new DX11SwapChain();
            swapShain.device = dxDevice;
            var desc = new SharpDX.DXGI.SwapChainDescription1()
            {
                Width = w,
                Height = h,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Stereo = false,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput | Usage.ShaderInput,
                BufferCount = 2,
                Scaling = SharpDX.DXGI.Scaling.None,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
                AlphaMode = AlphaMode.Premultiplied
            };

            swapShain.swapchain = new SwapChain1(dxDevice.Factory, dxDevice.Device, ref desc);
            swapShain.Initialize();
            return swapShain;
        }

        private void Initialize()
        {
            this.resource = Texture2D.FromSwapChain<Texture2D>(this.swapchain, 0);
            this.TextureDesc = this.resource.Description;

            this.RenderView = new RenderTargetView(device.Device, this.resource);
            this.RenderViewDesc = this.RenderView.Description;

            if (device.IsFeatureLevel11 && this.TextureDesc.SampleDescription.Count == 1)
            {
                this.UnorderedView = new UnorderedAccessView(device, this.resource);
            }
        }

        public void Resize()
        {
            this.Resize(0, 0);
        }

        public void Resize(int w, int h)
        {
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.RenderView != null) { this.RenderView.Dispose(); }
            this.resource.Dispose();

            this.swapchain.ResizeBuffers(1,w, h, SharpDX.DXGI.Format.Unknown, SwapChainFlags.AllowModeSwitch);

            this.resource = Texture2D.FromSwapChain<Texture2D>(this.swapchain, 0);

            this.TextureDesc = this.resource.Description;
            this.RenderView = new RenderTargetView(device.Device, this.resource);

            if (device.IsFeatureLevel11 && this.TextureDesc.SampleDescription.Count == 1)
            {
                this.UnorderedView = new UnorderedAccessView(device, this.resource);
            }
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
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.RenderView != null) { this.RenderView.Dispose(); }
            if (this.resource != null) { this.resource.Dispose(); }
            if (this.swapchain != null) { this.swapchain.Dispose(); }
        }
    }
}
