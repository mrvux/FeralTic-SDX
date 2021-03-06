﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;

namespace FeralTic.DX11.Resources
{
    public struct MsaaRenderTargetProperties
    {
        public int Width;
        public int Height;
        public SharpDX.DXGI.Format Format;
        public int SampleCount;
    }

    public class DX11RenderTarget2D : IDxTexture2D, IDxRenderTarget, IDxUnorderedResource
    {
        private DxDevice device;

        private Texture2DDescription resourceDesc;

        public Texture2D Texture { get; private set; }
        public ShaderResourceView ShaderView { get; private set; }

        public RenderTargetView RenderView { get; protected set; }

        public UnorderedAccessView UnorderedView { get; protected set; }

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

        public static DX11RenderTarget2D CreateTiled(DxDevice device, int w, int h, Format format, int mipLevels = 1)
        {
            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.Tiled,
                SampleDescription = new SampleDescription(1,0),
                Usage = ResourceUsage.Default,
                MipLevels = mipLevels
            };
            return new DX11RenderTarget2D(device, texBufferDesc);
        }

        public static DX11RenderTarget2D CreateMsaa(DxDevice device, MsaaRenderTargetProperties properties)
        {
            return new DX11RenderTarget2D(device, properties.Width, properties.Height, new SampleDescription(properties.SampleCount, 0), properties.Format, false, 1,false);
        }

        public static DX11RenderTarget2D CreateMsaaResolve(DxDevice device, MsaaRenderTargetProperties properties)
        {
            return new DX11RenderTarget2D(device, properties.Width, properties.Height, new SampleDescription(1, 0), properties.Format, false, 1, false);
        }

        public DX11RenderTarget2D(DxDevice device, int w, int h, SampleDescription sd, Format format, bool genMipMaps, int mmLevels) :
            this(device,w,h,sd,format,genMipMaps,mmLevels,true) {}

        public DX11RenderTarget2D(DxDevice device, int w, int h, SampleDescription sd, Format format) :
            this(device, w, h, sd, format, false, 1) { }

        public DX11RenderTarget2D(DxDevice device, DX11RenderTarget2D renderTarget) :
            this(device, renderTarget.Width,renderTarget.Height,renderTarget.resourceDesc.SampleDescription, renderTarget.Format)
        { }

        protected DX11RenderTarget2D(DxDevice device, Texture2DDescription desc)
        {
            bool allowUav = desc.BindFlags.HasFlag(BindFlags.UnorderedAccess);

            this.Texture = new Texture2D(device.Device, desc);
            this.resourceDesc = this.Texture.Description;

            this.RenderView = new RenderTargetView(device.Device, this.Texture);
            this.ShaderView = new ShaderResourceView(device.Device, this.Texture);

            if (desc.SampleDescription.Count == 1 && allowUav)
            {
                this.UnorderedView = new UnorderedAccessView(device.Device, this.Texture);
            }
        }

        public DX11RenderTarget2D(DxDevice device, int w, int h, SampleDescription sd, Format format, bool genMipMaps, int mmLevels, bool allowUAV)
        {
            this.device = device;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = sd,
                Usage = ResourceUsage.Default,
            };

            if (sd.Count == 1 && allowUAV)
            {
                texBufferDesc.BindFlags |= BindFlags.UnorderedAccess;
            }

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

            this.RenderView = new RenderTargetView(device.Device, this.Texture);
            this.ShaderView = new ShaderResourceView(device.Device, this.Texture);

            if (sd.Count == 1 && allowUAV)
            {
                this.UnorderedView = new UnorderedAccessView(device.Device, this.Texture);
            }
        }

        public void Clear(RenderContext context, Color4 color)
        {
            context.Context.ClearRenderTargetView(this.RenderView, color);
        }

        public void Dispose()
        {
            if (this.RenderView != null) { this.RenderView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
        }
    }
}
