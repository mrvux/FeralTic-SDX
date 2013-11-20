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
            DX11Device device = new DX11Device();
            DX11RenderContext immediate = new DX11RenderContext(device);
            immediate.Dispose();
            device.Dispose();
        }

        [TestMethod]
        public void ImmediateAndDefferedTest()
        {
            DX11Device device = new DX11Device();
            DX11RenderContext immediate = new DX11RenderContext(device);

            DX11DefferedRenderContext deffered = new DX11DefferedRenderContext(device);

            deffered.Dispose();
            immediate.Dispose();
            device.Dispose();
        }
    }
}
