using FeralTic.DX11;
using FeralTic.DX11.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Tests
{
    [TestClass]
    public class Texture1DTests : RenderDeviceTestBase
    {
        [TestMethod()]
        public void CreateDynamic()
        {
            DX11Texture1D result = DX11Texture1D.CreateDynamic(this.Device, 256, Format.R32_Float);

            Assert.IsNotNull(result.ShaderView, "Shader View Is Null");
            Assert.IsNull(result.UnorderedView, "Unoredered View is not null");
        }

        [TestMethod()]
        public void CreateWriteable()
        {
            DX11Texture1D result = DX11Texture1D.CreateWriteable(this.Device, 256, Format.R32_Float);

            Assert.IsNotNull(result.ShaderView, "Shader View Is Null");
            Assert.IsNotNull(result.UnorderedView, "Unoredered View is null");
        }

        [TestMethod()]
        public void CreateWriteableArray()
        {
            DX11Texture1DArray result = DX11Texture1DArray.CreateWriteable(this.Device, 256,4, Format.R32_Float);

            Assert.IsNotNull(result.ShaderView, "Shader View Is Null");
            Assert.IsNotNull(result.UnorderedView, "Unoredered View is null");
        }


    }
}
