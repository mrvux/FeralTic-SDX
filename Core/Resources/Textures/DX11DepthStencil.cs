using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Direct3D;


namespace FeralTic.DX11.Resources
{
    

    public class DX11DepthStencil : IDX11Texture2D, IDX11DepthStencil
    {
        private DxDevice device;
        
        private Texture2DDescription resourceDesc;

        public DepthStencilView DepthView { get; protected set; }
        public DepthStencilView ReadOnlyView { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public Texture2D Texture { get; private set; }

        public DX11Texture2D Stencil { get; private set; }

        public int Width
        {
            get { return this.resourceDesc.Width; }
        }

        public int Height
        {
            get { return this.resourceDesc.Height; }
        }

        public Format Format
        {
            get { return this.resourceDesc.Format; }
        }

        public eDepthFormat DepthFormat { get; private set; }


        public DX11DepthStencil(DxDevice device, int w, int h, eDepthFormat depthformat = eDepthFormat.d24s8)
            : this(device, w, h, new SampleDescription(1, 0), depthformat)
        {

        }

        public DX11DepthStencil(DxDevice device, int w, int h, SampleDescription sd, eDepthFormat depthformat = eDepthFormat.d24s8)
        {
            this.device = device;
            var depthBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = depthformat.GetTypeLessFormat(),
                Height = h,
                Width = w,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = sd,
                Usage = ResourceUsage.Default
            };

            this.Texture = new Texture2D(device.Device, depthBufferDesc);
            this.resourceDesc = this.Texture.Description;
            this.DepthFormat = depthformat;

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Format = depthformat.GetSRVFormat(),
                Dimension = sd.Count == 1 ? ShaderResourceViewDimension.Texture2D : ShaderResourceViewDimension.Texture2DMultisampled,
            };

            if (sd.Count == 1)
            {
                srvd.Texture2D.MipLevels = 1;
                srvd.Texture2D.MostDetailedMip = 0;
            }

            this.ShaderView = new ShaderResourceView(device.Device, this.Texture, srvd);


            if (depthformat.HasStencil())
            {
                ShaderResourceViewDescription stencild = new ShaderResourceViewDescription()
                {
                    Dimension = sd.Count == 1 ? ShaderResourceViewDimension.Texture2D : ShaderResourceViewDimension.Texture2DMultisampled,
                    Format = depthformat.GetStencilSRVFormat(),
                };

                if (sd.Count == 1)
                {
                    stencild.Texture2D.MipLevels = 1;
                    stencild.Texture2D.MostDetailedMip = 0;
                }

                ShaderResourceView stencilview = new ShaderResourceView(this.device.Device, this.Texture, stencild);

                this.Stencil = DX11Texture2D.FromReference(this.device, this.Texture, stencilview);

            }
            else
            {
                //Just pass depth instead
                this.Stencil = null;
            }

            DepthStencilViewDescription dsvd = new DepthStencilViewDescription()
            {
                Format = depthformat.GetDepthFormat(),
                Dimension = sd.Count == 1 ? DepthStencilViewDimension.Texture2D : DepthStencilViewDimension.Texture2DMultisampled,
            };

            this.DepthView = new DepthStencilView(device.Device, this.Texture, dsvd);

            //Read only dsv only supported in dx11 minimum
            if (device.IsFeatureLevel11)
            {
                dsvd.Flags = DepthStencilViewFlags.ReadOnlyDepth;
                if (depthformat.HasStencil()) { dsvd.Flags |= DepthStencilViewFlags.ReadOnlyStencil; }

                this.ReadOnlyView = new DepthStencilView(device.Device, this.Texture, dsvd);
            }

        }

        public void Clear(DX11RenderContext context, bool cleardepth = true, bool clearstencil = true, float depth = 1.0f, byte stencil = 0)
        {
            if (cleardepth || clearstencil)
            {
                DepthStencilClearFlags flags = (DepthStencilClearFlags)0;
                if (cleardepth) { flags = DepthStencilClearFlags.Depth; }
                if (clearstencil) { flags |= DepthStencilClearFlags.Stencil; }

                context.Context.ClearDepthStencilView(this.DepthView, flags, depth, stencil);
            }
        }

        public void Dispose()
        {
            if (this.Stencil != null) { this.Stencil.Dispose(); }
            if (this.DepthView != null) { this.DepthView.Dispose(); }
            if (this.ReadOnlyView != null) { this.ReadOnlyView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
        }
    }
}
