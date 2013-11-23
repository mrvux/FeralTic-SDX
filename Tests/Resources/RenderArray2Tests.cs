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
    public class RenderTextureArrayTests : RenderDeviceTestBase
    {
        [TestMethod()]
        public void CreateSimple()
        {
            DX11RenderTextureArray array = new DX11RenderTextureArray(this.Device, 256, 256, 4, Format.R8G8B8A8_UNorm, false);

            Assert.IsNotNull(array.RenderView);
            Assert.IsNotNull(array.ShaderView);
            Assert.IsNull(array.Slices);

            array.Dispose();
        }

        [TestMethod()]
        public void CreateWithSlices()
        {
            int slicecount = 4;
            DX11RenderTextureArray array = new DX11RenderTextureArray(this.Device, 256, 256, slicecount, Format.R8G8B8A8_UNorm, true);

            Assert.IsNotNull(array.RenderView);
            Assert.IsNotNull(array.ShaderView);
            Assert.IsNotNull(array.Slices);
            Assert.AreEqual(slicecount, array.Slices.Length);

            array.Dispose();
        }
    }
}
