using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeralTic.DX11;

namespace FeralTic.Tests
{
    [TestClass]
    public class DeviceTest
    {
        [TestMethod]
        public void CreateDevice()
        {
            DX11Device device = new DX11Device();
            device.Dispose();
        }

        [TestMethod]
        public void CreateRenderDevice()
        {
            RenderDevice device = new RenderDevice();
            device.Dispose();
        }       
    }
}
