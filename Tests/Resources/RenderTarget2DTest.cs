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
    public class RenderTarget2DTests : RenderDeviceTestBase
    {
        [TestMethod()]
        public void CreateSimple()
        {
            DX11RenderTarget2D rt = new DX11RenderTarget2D(this.Device, 320, 240, new SampleDescription(1, 0), SharpDX.DXGI.Format.B8G8R8A8_UNorm, false, 0, false);

            Assert.IsNull(rt.UnorderedView);

            rt.Dispose();
        }

        [TestMethod()]
        public void CreateWithUAV()
        {
            DX11RenderTarget2D rt = new DX11RenderTarget2D(this.Device, 320, 240, new SampleDescription(1, 0), SharpDX.DXGI.Format.R8G8B8A8_UNorm, false, 0, true);

            Assert.IsNotNull(rt.UnorderedView);

            rt.Dispose();
        }

        [TestMethod()]
        public void CreateMS()
        {
            DX11RenderTarget2D rt = new DX11RenderTarget2D(this.Device, 320, 240, new SampleDescription(2, 0), SharpDX.DXGI.Format.R8G8B8A8_UNorm, false, 0, false);
            Assert.IsNull(rt.UnorderedView);
            rt.Dispose();

            //Since we have multisampled version, flag for uav should be ignored
            rt = new DX11RenderTarget2D(this.Device, 320, 240, new SampleDescription(2, 0), SharpDX.DXGI.Format.R8G8B8A8_UNorm, false, 0, true);
            Assert.IsNull(rt.UnorderedView);
            rt.Dispose();
            
        }
    }
}
