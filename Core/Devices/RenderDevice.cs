using FeralTic.DX11;
using FeralTic.DX11.Geometry;
using SharpDX.Direct3D11;
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

        public DX11ResourcePoolManager ResourcePool { get; private set; }

        public DX11PrimitivesManager Primitives { get; private set; }

        public RenderDevice(DeviceCreationFlags flags = DeviceCreationFlags.BgraSupport, int adapterindex = 0)
            : base(flags,adapterindex)
        {

        }



        protected override void OnLoad()
        {
            this.BlendStates = new BlendStates(this);
            this.DepthStencilStates = new DepthStencilStates(this);
            this.RasterizerStates = new RasterizerStates(this);
            this.SamplerStates = new SamplerStates(this);

            this.DefaultTextures = new DefaultTextures(this);

            this.ResourcePool = new DX11ResourcePoolManager(this);

            this.Primitives = new DX11PrimitivesManager(this);
        }

        protected override void OnDispose()
        {
            this.BlendStates.Dispose();
            this.RasterizerStates.Dispose();
            this.DepthStencilStates.Dispose();
            this.SamplerStates.Dispose();

            this.DefaultTextures.Dispose();

            this.ResourcePool.Dispose();
        }

    }
}
