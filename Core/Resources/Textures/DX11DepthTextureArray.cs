using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using FeralTic.DX11.Utils;
using SharpDX.Direct3D;


namespace FeralTic.DX11.Resources
{
    public class DX11DepthTextureArray : IDxTexture2D, IDxDepthStencil
    {
        public DepthStencilView DepthView { get; protected set; }
        public DepthStencilView ReadOnlyView { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public Texture2D Texture { get; private set; }

        public DX11Texture2D Stencil { get { return null; } }
        
        public DX11SliceDepthStencil[] SliceDepthView { get; protected set; }

        public int ElementCount { get { return resourceDesc.ArraySize; } }

        private DxDevice device;
        private Texture2DDescription resourceDesc;

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

        public DX11DepthTextureArray(DxDevice device, int w, int h, int elemcnt, eDepthFormat depthformat, bool buildslices = true)
        {
            this.device = device;
            this.DepthFormat = depthformat;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = elemcnt,
                BindFlags = BindFlags.DepthStencil | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = depthformat.GetTypeLessFormat(),
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1,0),
                Usage = ResourceUsage.Default,
            };

            this.Texture = new Texture2D(device, texBufferDesc);
            this.resourceDesc = this.Texture.Description;

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Format = depthformat.GetSRVFormat(),
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                Texture2DArray = new ShaderResourceViewDescription.Texture2DArrayResource()
                {
                    ArraySize = elemcnt,
                    FirstArraySlice = 0,
                    MipLevels = 1,
                    MostDetailedMip = 0
                }
            };

            this.ShaderView = new ShaderResourceView(device.Device, this.Texture, srvd);

            DepthStencilViewDescription dsvd = new DepthStencilViewDescription()
            {
                Format = depthformat.GetDepthFormat(),
                Dimension = DepthStencilViewDimension.Texture2DArray,
                Texture2DArray = new DepthStencilViewDescription.Texture2DArrayResource()
                {
                    ArraySize = elemcnt,
                    FirstArraySlice = 0,
                    MipSlice = 0
                }
            };

            this.DepthView = new DepthStencilView(device, this.Texture, dsvd);

            dsvd.Flags = DepthStencilViewFlags.ReadOnlyDepth;
            if (depthformat.HasStencil()) { dsvd.Flags |= DepthStencilViewFlags.ReadOnlyStencil; }

            this.ReadOnlyView = new DepthStencilView(device.Device, this.Texture, dsvd);

            this.SliceDepthView = new DX11SliceDepthStencil[this.ElementCount];

            if (buildslices)
            {
                for (int i = 0; i < this.ElementCount; i++)
                {
                    this.SliceDepthView[i] = new DX11SliceDepthStencil(this.device, this, i, depthformat);
                }
            }
        }

        public void Dispose()
        {
            if (this.DepthView != null) { this.DepthView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
            if (this.ReadOnlyView != null) { this.ReadOnlyView.Dispose(); }
        }
    }
}
