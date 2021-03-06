﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

namespace FeralTic.DX11
{
    public class SamplerStates : RenderStates<SamplerState>
    {
        private DxDevice device;

        public SamplerState LinearWrap { get; private set; }
        public SamplerState LinearClamp { get; private set; }
        public SamplerState LinearMirror { get; private set; }
        public SamplerState PointWrap { get; private set; }
        public SamplerState PointClamp { get; private set; }
        public SamplerState LinearBorder { get; private set; }
        public SamplerState PointBorder { get; private set; }
        public SamplerState PointMirror { get; private set; }

        public SamplerStates(DxDevice device)
        {
            this.device = device;
            this.Initialize();
        }

        public override string EnumName
        {
            get
            {
                return "DX11.SamplerPresets";
            }
        }

        protected override void Initialize()
        {
            this.CreateLinearWrap();
            this.CreateLinearBorder();
            this.CreateLinearClamp();
            this.CreateLinearMirror();

            this.CreatePointBorder();
            this.CreatePointClamp();
            this.CreatePointWrap();
            this.CreatePointMirror();
        }

        private void CreateLinearWrap()
        {
            SamplerStateDescription sd = new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                ComparisonFunction = Comparison.Always,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipLinear
            };

            this.LinearWrap = new SamplerState(device.Device, sd);
            this.AddState("LinearWrap", LinearWrap);
        }

        private void CreateLinearClamp()
        {
            SamplerStateDescription sd = new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                ComparisonFunction = Comparison.Always,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipLinear
            };
            this.LinearClamp = new SamplerState(device.Device, sd);
            this.AddState("LinearClamp", LinearClamp);
        }

        private void CreateLinearBorder()
        {
            SamplerStateDescription sd = new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                AddressW = TextureAddressMode.Border,
                ComparisonFunction = Comparison.Always,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipLinear,
                BorderColor = new SharpDX.Color4(0, 0, 0, 0)
            };
            this.LinearBorder = new SamplerState(device.Device, sd);
            this.AddState("LinearBorder", LinearBorder);
        }

        private void CreatePointWrap()
        {
            SamplerStateDescription sd = new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                ComparisonFunction = Comparison.Always,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipPoint
            };
            this.PointWrap = new SamplerState(device.Device, sd);
            this.AddState("PointWrap", PointWrap);
        }

        private void CreatePointClamp()
        {
            SamplerStateDescription sd = new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                ComparisonFunction = Comparison.Always,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipPoint
            };
            this.PointClamp = new SamplerState(device.Device, sd);
            this.AddState("PointClamp", PointClamp);
        }

        private void CreatePointBorder()
        {
            SamplerStateDescription sd = new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                AddressW = TextureAddressMode.Border,
                ComparisonFunction = Comparison.Always,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipPoint,
                BorderColor = new SharpDX.Color4(0, 0, 0, 0)
            };
            this.PointBorder = new SamplerState(device.Device, sd);
            this.AddState("PointBorder", PointBorder);
        }

        private void CreatePointMirror()
        {
            SamplerStateDescription sd = new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Mirror,
                AddressV = TextureAddressMode.Mirror,
                AddressW = TextureAddressMode.Mirror,
                ComparisonFunction = Comparison.Always,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipPoint,
                BorderColor = new SharpDX.Color4(0, 0, 0, 0)
            };
            this.PointMirror = new SamplerState(device.Device, sd);
            this.AddState("PointMirror", PointMirror);
        }

        private void CreateLinearMirror()
        {
            SamplerStateDescription sd = new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Mirror,
                AddressV = TextureAddressMode.Mirror,
                AddressW = TextureAddressMode.Mirror,
                ComparisonFunction = Comparison.Always,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipLinear,
                BorderColor = new SharpDX.Color4(0, 0, 0, 0)
            };
            this.LinearMirror = new SamplerState(device.Device, sd);
            this.AddState("LinearMirror", LinearMirror);
        }
    }
}
