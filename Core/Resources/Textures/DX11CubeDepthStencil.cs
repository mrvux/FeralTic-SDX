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
    public class DX11CubeDepthStencil : DX11Texture2D, IDxDepthStencil
    {
        private DxDevice device;

        public DepthStencilView DepthView { get; protected set; }
        public DepthStencilView ReadOnlyView { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public Texture2D Texture { get; protected set; }

        public DX11SliceDepthStencil[] SliceDepthViews { get; protected set; }

        private Texture2DDescription resourceDesc;

        public int Width
        {
            get { return resourceDesc.Width; }
        }

        public int Height
        {
            get { return resourceDesc.Height; }
        }

        public Format Format
        {
            get { return resourceDesc.Format; }
        }

        public DX11CubeDepthStencil(DxDevice device, int size, SampleDescription sd, eDepthFormat depthformat)
        {
            this.device = device;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 6,
                BindFlags = BindFlags.DepthStencil | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = depthformat.GetTypeLessFormat(),
                Height = size,
                Width = size,
                OptionFlags = ResourceOptionFlags.TextureCube,
                SampleDescription = sd,
                Usage = ResourceUsage.Default,
                MipLevels = 1
            };

            this.Texture = new Texture2D(device, texBufferDesc);
            this.resourceDesc = this.Texture.Description;

            ShaderResourceViewDescription svd = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.TextureCube,
                Format = depthformat.GetSRVFormat(),
                TextureCube = new ShaderResourceViewDescription.TextureCubeResource()
                {
                    MipLevels = 1,
                    MostDetailedMip = 0
                }
            };

            DepthStencilViewDescription dsvd = new DepthStencilViewDescription()
            {
                Format = depthformat.GetDepthFormat(),
                Dimension = DepthStencilViewDimension.Texture2DArray,
                Texture2DArray = new DepthStencilViewDescription.Texture2DArrayResource()
                {
                    ArraySize = 6,
                    FirstArraySlice = 0,
                    MipSlice = 0
                }
            };

            this.DepthView = new DepthStencilView(device, this.Texture, dsvd);

            if (device.IsFeatureLevel11)
            {
                dsvd.Flags = DepthStencilViewFlags.ReadOnlyDepth;
                if (depthformat.HasStencil()) { dsvd.Flags |= DepthStencilViewFlags.ReadOnlyStencil; }

                this.ReadOnlyView = new DepthStencilView(device, this.Texture, dsvd);
            }

            this.ShaderView = new ShaderResourceView(device, this.Texture, svd);

            this.SliceDepthViews = new DX11SliceDepthStencil[6];
            for (int i = 0; i < 6; i++)
            {
                this.SliceDepthViews[i] = new DX11SliceDepthStencil(device, this, i, depthformat);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < 6; i++)
            {
                this.SliceDepthViews[i].Dispose();
            }
            this.DepthView.Dispose();
            this.ShaderView.Dispose();
            if (this.ReadOnlyView != null) { this.ReadOnlyView.Dispose(); }
            this.Texture.Dispose();
        }
    }
}
