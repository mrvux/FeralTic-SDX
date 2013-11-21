using FeralTic.DX11.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Tests
{
    [TestClass()]
    public class ResourcePoolTest : RenderDeviceTestBase
    {
        [TestMethod()]
        public void TestSimilarTarget()
        {
            DX11RenderTarget2D rt1 = this.Device.ResourcePool.LockRenderTarget(100, 100, SharpDX.DXGI.Format.R8G8B8A8_UNorm);

            this.Device.ResourcePool.Unlock(rt1);

            DX11RenderTarget2D rt2 = this.Device.ResourcePool.LockRenderTarget(100, 100, SharpDX.DXGI.Format.R8G8B8A8_UNorm);

            Assert.AreEqual(rt1.Texture.NativePointer, rt2.Texture.NativePointer);
        }

        [TestMethod()]
        public void TestTwoTarget()
        {
            DX11RenderTarget2D rt1 = this.Device.ResourcePool.LockRenderTarget(100, 100, SharpDX.DXGI.Format.R8G8B8A8_UNorm);

            this.Device.ResourcePool.Unlock(rt1);

            DX11RenderTarget2D rt2 = this.Device.ResourcePool.LockRenderTarget(120, 100, SharpDX.DXGI.Format.R8G8B8A8_UNorm);

            Assert.AreNotEqual(rt1.Texture.NativePointer, rt2.Texture.NativePointer);
        }
    }
}
