using FeralTic.DX11;
using FeralTic.DX11.Geometry;
using FeralTic.DX11.Resources;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public class RenderDevice : DxDevice
    {
        public BlendStates BlendStates { get; private set; }

        public RasterizerStates RasterizerStates { get; private set; }

        public DepthStencilStates DepthStencilStates { get; private set; }

        public SamplerStates SamplerStates { get; private set; }

        public DefaultTextures DefaultTextures { get; private set; }

        public ResourcePoolManager ResourcePool { get; private set; }

        public PrimitivesManager Primitives { get; private set; }

        public ResourceScheduler ResourceScheduler { get; private set; }

        public TiledResourcePool SharedTiledPool { get; private set; }

        public bool HasBufferSupport { get; private set; }

        public RenderDevice(DeviceCreationFlags flags = DeviceCreationFlags.BgraSupport, int adapterindex = 0)
            : base(flags,adapterindex)
        {
            //Since flag doesn't report buffer support on win7, we just try to create a small buffer
            try
            {
                DX11RawBuffer b = DX11RawBuffer.CreateWriteable(this, 16, new RawBufferBindings() { WriteMode = eRawBufferWriteMode.Uav });
                b.Dispose();
                this.HasBufferSupport = true;
            }
            catch 
            {
                this.HasBufferSupport = false;
            }       
        }


        public bool IsSupported(FormatSupport usage, Format format)
        {
            FormatSupport support = this.Device.CheckFormatSupport(format);
            return (support | usage) == support;
        }

        protected override void OnLoad()
        {
            this.BlendStates = new BlendStates(this);
            this.DepthStencilStates = new DepthStencilStates(this);
            this.RasterizerStates = new RasterizerStates(this);
            this.SamplerStates = new SamplerStates(this);

            this.DefaultTextures = new DefaultTextures(this);

            this.ResourcePool = new ResourcePoolManager(this);

            this.Primitives = new PrimitivesManager(this);
            this.ResourceScheduler = new ResourceScheduler(this, 3);
            this.ResourceScheduler.Initialize();

            this.SharedTiledPool = new TiledResourcePool(this);
                 
        }

        protected override void OnDispose()
        {
            this.BlendStates.Dispose();
            this.RasterizerStates.Dispose();
            this.DepthStencilStates.Dispose();
            this.SamplerStates.Dispose();

            this.DefaultTextures.Dispose();

            this.ResourcePool.Dispose();
            this.ResourceScheduler.Dispose();
            this.Primitives.Dispose();

            this.SharedTiledPool.Dispose();
        }

    }
}
