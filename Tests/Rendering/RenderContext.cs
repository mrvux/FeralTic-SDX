using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeralTic.DX11;

namespace FeralTic.Tests.Rendering
{
    [TestClass]
    public class RenderContextTest
    {
        [TestMethod]
        public void ImmediateTest()
        {
            DxDevice device = new DxDevice();
            RenderContext immediate = new RenderContext(device);
            immediate.Dispose();
            device.Dispose();
        }

        [TestMethod]
        public void ImmediateAndDefferedTest()
        {
            DxDevice device = new DxDevice();
            RenderContext immediate = new RenderContext(device);

            DefferedRenderContext deffered = new DefferedRenderContext(device);

            deffered.Dispose();
            immediate.Dispose();
            device.Dispose();
        }
    }
}
