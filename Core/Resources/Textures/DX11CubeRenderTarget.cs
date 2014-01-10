using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Device = SharpDX.Direct3D11.Device;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Resources
{
    public class DX11CubeRenderTarget : IDxTexture2D, IDxRenderTarget
    {
        private DxDevice device;

        public RenderTargetView RenderView { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public Texture2D Texture { get; protected set; }

        public DX11SliceRenderTarget[] Slices { get; protected set; }

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

        public DX11CubeRenderTarget(DxDevice device, int size, SampleDescription sd, Format format, bool genMipMaps, int mmLevels)
        {
            this.device = device;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 6,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = size,
                Width = size,
                OptionFlags = ResourceOptionFlags.TextureCube,
                SampleDescription = sd,
                Usage = ResourceUsage.Default,
            };

            if (genMipMaps && sd.Count == 1)
            {
                texBufferDesc.OptionFlags |= ResourceOptionFlags.GenerateMipMaps;
                texBufferDesc.MipLevels = mmLevels;
            }
            else
            {
                //Make sure we enforce 1 here, as we dont generate
                texBufferDesc.MipLevels = 1;
            }

            this.Texture = new Texture2D(device.Device, texBufferDesc);
            this.resourceDesc = this.Texture.Description;

            //Create faces SRV/RTV
            this.Slices = new DX11SliceRenderTarget[6];

            ShaderResourceViewDescription svd = new ShaderResourceViewDescription()
            {
                Format = format,
                Dimension = ShaderResourceViewDimension.TextureCube,
                TextureCube= new ShaderResourceViewDescription.TextureCubeResource()
                {
                    MipLevels = 1,
                    MostDetailedMip = 0,
                }
            };

            RenderTargetViewDescription rtvd = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture2DArray,
                Format = format,
                Texture2DArray = new RenderTargetViewDescription.Texture2DArrayResource()
                {
                    ArraySize = 6,
                    FirstArraySlice = 0,
                    MipSlice  =0
                }
            };

            this.RenderView = new RenderTargetView(device.Device, this.Texture, rtvd);

            this.ShaderView = new ShaderResourceView(device.Device, this.Texture, svd);

            for (int i = 0; i < 6; i++)
            {
                this.Slices[i] = new DX11SliceRenderTarget(device, this, i);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < 6; i++)
            {
                this.Slices[i].Dispose();
            }
            if (this.RenderView != null) { this.RenderView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }

        }
    }
}
