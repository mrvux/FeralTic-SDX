using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

namespace FeralTic.DX11
{
    public class RasterizerStates : RenderStates<RasterizerState>
    {
        private DxDevice device;

        public RasterizerState BackCullSolid { get; private set; }
        public RasterizerState FrontCullSolid { get; private set; }
        public RasterizerState NoCullSolid { get; private set; }
        public RasterizerState WireFrame { get; private set; }
        public RasterizerState LineAlpha { get; private set; }
        public RasterizerState LineQuadrilateral { get; private set; }
        public RasterizerState ScissorClip { get; private set; }

        public RasterizerStates(DxDevice device)
        {
            this.device = device;
            this.Initialize();
        }

        public override string EnumName
        {
            get
            {
                return "DX11.RasterizerPresets";
            }
        }

        protected override void Initialize()
        {
            this.CreateBackCullSimple();
            this.CreateFrontCullSimple();
            this.CreateNoCullSimple();
            this.CreateLine();
        }

        private void CreateNoCullSimple()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f             
            };

            this.NoCullSolid = new RasterizerState(device.Device, rsd);
            this.AddState("NoCullSimple",this.NoCullSolid);

            rsd.FillMode = FillMode.Wireframe;
            this.WireFrame = new RasterizerState(device.Device, rsd);

            this.AddState("NoCullWireframe", this.WireFrame);
        }

        public RasterizerState CreateNoCullSimple(int bias, float biasClamp)
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                DepthBias = bias,
                DepthBiasClamp = biasClamp,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };

            return new RasterizerState(device.Device, rsd);
        }

        private void CreateBackCullSimple()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
            this.BackCullSolid = new RasterizerState(device.Device, rsd);
            this.AddState("BackCullSimple",this.BackCullSolid);

            rsd.FillMode = FillMode.Wireframe;
            this.AddState("BackCullWireframe", new RasterizerState(device.Device, rsd));

            rsd.FillMode = FillMode.Solid;
            rsd.IsScissorEnabled = true;
            rsd.IsDepthClipEnabled = false;
            rsd.CullMode = CullMode.None;
            this.ScissorClip = new RasterizerState(device.Device, rsd);
            this.AddState("ScissorClip", this.ScissorClip);
        }

        private void CreateFrontCullSimple()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.Front,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
            this.FrontCullSolid = new RasterizerState(device.Device, rsd);
            this.AddState("FrontCullSimple", this.FrontCullSolid);

            rsd.FillMode = FillMode.Wireframe;
            this.AddState("FrontCullWireframe", new RasterizerState(device.Device, rsd));
        }

        private void CreateLine()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = true,
                IsDepthClipEnabled = true,
                IsFrontCounterClockwise = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };

            this.LineAlpha = new RasterizerState(device.Device, rsd);
            this.AddState("LineAlpha", this.LineAlpha);

            rsd.IsMultisampleEnabled = true;
            this.LineQuadrilateral = new RasterizerState(device, rsd);
            this.AddState("LineQuadrilateral", this.LineQuadrilateral);
        }
    }
}
