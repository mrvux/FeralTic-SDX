using FeralTic.DX11;
using FeralTic.DX11.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Tests.Resources
{
    [TestClass]
    public class DepthStencilTests
    {
        protected DX11Device Device { get; set; }

        [TestInitialize()]
        public void Initialize()
        {
            this.Device = new DX11Device();
        }

        [TestCleanup()]
        public void CleanUp()
        {
            if (Device != null) { Device.Dispose(); }
        }

        [TestMethod()]
        public void CreateD16()
        {
            DX11DepthStencil d = new DX11DepthStencil(this.Device, 320, 240, eDepthFormat.d16);

            Assert.IsNotNull(d.Texture, "Texture Is Null");
            Assert.IsNotNull(d.ShaderView, "Shader View Is Null");
            Assert.IsNotNull(d.DepthView, "Depth View Is Null");
            if (Device.IsFeatureLevel11)
            {
                Assert.IsNotNull(d.ReadOnlyView, "Read Only View Is Null");
            }
            else
            {
                Assert.IsNull(d.ReadOnlyView, "Read Only View Should Be Null");
            }
            Assert.IsNull(d.Stencil, "Stencil is Not Null");

            d.Dispose();
        }

        [TestMethod()]
        public void CreateD32()
        {
            DX11DepthStencil d = new DX11DepthStencil(this.Device, 320, 240, eDepthFormat.d32);

            Assert.IsNotNull(d.Texture, "Texture Is Null");
            Assert.IsNotNull(d.ShaderView, "Shader View Is Null");
            Assert.IsNotNull(d.DepthView, "Depth View Is Null");
            if (Device.IsFeatureLevel11)
            {
                Assert.IsNotNull(d.ReadOnlyView, "Read Only View Is Null");
            }
            else
            {
                Assert.IsNull(d.ReadOnlyView, "Read Only View Should Be Null");
            }
            Assert.IsNull(d.Stencil, "Stencil is Not Null");

            d.Dispose();
        }

        [TestMethod()]
        public void CreateD24S8()
        {
            DX11DepthStencil d = new DX11DepthStencil(this.Device, 320, 240, eDepthFormat.d24s8);

            Assert.IsNotNull(d.Texture, "Texture Is Null");
            Assert.IsNotNull(d.ShaderView, "Shader View Is Null");
            Assert.IsNotNull(d.DepthView, "Depth View Is Null");

            if (Device.IsFeatureLevel11)
            {
                Assert.IsNotNull(d.ReadOnlyView, "Read Only View Is Null");
            }
            else
            {
                Assert.IsNull(d.ReadOnlyView, "Read Only View Should Be Null");
            }
            Assert.IsNotNull(d.Stencil, "Stencil is Null");

            d.Dispose();
        }

        [TestMethod()]
        public void CreateD32S8()
        {
            DX11DepthStencil d = new DX11DepthStencil(this.Device, 320, 240, eDepthFormat.d32s8);

            Assert.IsNotNull(d.Texture, "Texture Is Null");
            Assert.IsNotNull(d.ShaderView, "Shader View Is Null");
            Assert.IsNotNull(d.DepthView, "Depth View Is Null");

            if (Device.IsFeatureLevel11)
            {
                Assert.IsNotNull(d.ReadOnlyView, "Read Only View Is Null");
            }
            else
            {
                Assert.IsNull(d.ReadOnlyView, "Read Only View Should Be Null");
            }
            Assert.IsNotNull(d.Stencil, "Stencil is Null");

            d.Dispose();
        }
    }
}
