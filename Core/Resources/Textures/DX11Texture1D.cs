﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using SharpDX.Direct3D11;
using SharpDX;

namespace FeralTic.DX11.Resources
{
    public class DX11Texture1D : IDX11Texture1D, IDX11UnorderedResource
    {
        private DX11Device device;

        public Texture1D Texture { get; private set; }

        public ShaderResourceView ShaderView { get; private set; }
        public UnorderedAccessView UnorderedView { get; private set; }

        private Texture1DDescription description;

        public int Width 
        { 
            get { return this.description.Width; }        
        }

        public SharpDX.DXGI.Format Format
        {
            get { return this.description.Format; }
        }

        protected DX11Texture1D(DX11Device device, Texture1DDescription desc)
        {
            this.device = device;
            this.description = desc;
            this.Texture = new Texture1D(device.Device, desc);
            this.ShaderView = new ShaderResourceView(device.Device, this.Texture);

            if (desc.BindFlags.HasFlag(BindFlags.UnorderedAccess))
            {
                this.UnorderedView = new UnorderedAccessView(device.Device, this.Texture);
            }
        }

        public DataStream MapForWrite(DX11RenderContext context)
        {
            DataStream ds;
            context.Context.MapSubresource(this.Texture, 0, MapMode.WriteDiscard, MapFlags.None, out ds);
            return ds;
        }

        public void Unmap(DX11RenderContext context)
        {
            context.Context.UnmapSubresource(this.Texture, 0);
        }

        public void WriteData<T>(DX11RenderContext context, T[] data) where T : struct
        {
            DataStream ds = this.MapForWrite(context);
            ds.WriteRange<T>(data);
            this.Unmap(context);
        }

        public static DX11Texture1D CreateDynamic(DX11Device device, int width, SharpDX.DXGI.Format format)
        {
            Texture1DDescription desc = new Texture1DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                Width = width,
                Format = format,
                MipLevels = 1,
                Usage = ResourceUsage.Dynamic
            };
            return new DX11Texture1D(device, desc);
        }

        public static DX11Texture1D CreateWriteable(DX11Device device, int width, SharpDX.DXGI.Format format)
        {
            Texture1DDescription desc = new Texture1DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Width = width,
                MipLevels = 1,
                Format = format,
                Usage = ResourceUsage.Default
            };
            return new DX11Texture1D(device, desc);
        }

        public void Dispose()
        {
            if (this.UnorderedView != null) { this.UnorderedView.Dispose(); }
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
        }
    }
}