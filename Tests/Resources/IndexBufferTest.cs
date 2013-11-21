using FeralTic.DX11;
using FeralTic.DX11.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Tests
{
    [TestClass]
    public class IndexBufferTests : RenderDeviceTestBase
    {
        [TestMethod()]
        public void CreateImmutableUint()
        {
            uint[] ud = new uint[] { 0,1,2,3,4,5,6};
            DX11IndexBuffer ibo = DX11IndexBuffer.CreateImmutable(Device, ud);

            Assert.IsNotNull(ibo.Buffer, "Buffer Is Null");
            ibo.Dispose();
        }

        [TestMethod()]
        public void CreateImmutableInt()
        {
            int[] ud = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            DX11IndexBuffer ibo = DX11IndexBuffer.CreateImmutable(Device, ud);

            Assert.IsNotNull(ibo.Buffer, "Buffer Is Null");
            ibo.Dispose();
        }


        [TestMethod()]
        public void CreateImmutableShort()
        {
            short[] ud = new short[] { 0, 1, 2, 3, 4, 5, 6 };
            DX11IndexBuffer ibo = DX11IndexBuffer.CreateImmutable(Device, ud);

            Assert.IsNotNull(ibo.Buffer, "Buffer Is Null");
            ibo.Dispose();
        }
    }
}
