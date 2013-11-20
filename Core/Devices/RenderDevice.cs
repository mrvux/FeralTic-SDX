﻿using FeralTic.DX11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public class RenderDevice : DX11Device
    {
        public DX11BlendStates BlendStates { get; private set; }

        public DX11RasterizerStates RasterizerStates { get; private set; }

        public DX11DepthStencilStates DepthStencilStates { get; private set; }

        public DX11SamplerStates SamplerStates { get; private set; }

        protected override void OnLoad()
        {
            this.BlendStates = new DX11BlendStates(this);
            this.DepthStencilStates = new DX11DepthStencilStates(this);
            this.RasterizerStates = new DX11RasterizerStates(this);
            this.SamplerStates = new DX11SamplerStates(this);
        }

        protected override void OnDispose()
        {
            this.BlendStates.Dispose();
            this.RasterizerStates.Dispose();
            this.DepthStencilStates.Dispose();
            this.SamplerStates.Dispose();
        }

    }
}
