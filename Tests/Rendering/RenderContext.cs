using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeralTic.DX11;

namespace FeralTic.Tests.Rendering
{
    [TestClass]
    public class RenderContext
    {
        [TestMethod]
        public void ImmediateTest()
        {
            DxDevice device = new DxDevice();
            DX11RenderContext immediate = new DX11RenderContext(device);
            immediate.Dispose();
            device.Dispose();
        }

        [TestMethod]
        public void ImmediateAndDefferedTest()
        {
            DxDevice device = new DxDevice();
            DX11RenderContext immediate = new DX11RenderContext(device);

            DX11DefferedRenderContext deffered = new DX11DefferedRenderContext(device);

            deffered.Dispose();
            immediate.Dispose();
            device.Dispose();
        }
    }
}
