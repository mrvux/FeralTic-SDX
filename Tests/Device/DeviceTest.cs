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
            DxDevice device = new DxDevice();
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
