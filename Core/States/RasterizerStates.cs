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
    }
}
