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

        [TestMethod()]
        public void StructuredCopyTest()
        {
            uint[] ud = new uint[] { 0, 1, 2, 3, 4, 5, 6 };
            DX11IndexBuffer ibo = DX11IndexBuffer.CreateImmutable(Device, ud);

            DX11StructuredBuffer sb = ibo.CopyToStructuredBuffer(this.RenderContext);
            DX11StructuredBuffer staging = sb.AsStaging();
            this.RenderContext.Context.CopyResource(sb.Buffer, staging.Buffer);

            uint[] data = staging.ReadData<uint>(this.RenderContext);

            ibo.Dispose();
            sb.Dispose();
            staging.Dispose();

            Assert.AreEqual(ud.Length, data.Length, "Resources are or different Size");
            for (int i = 0; i < ud.Length;i++)
            {
                Assert.AreEqual(ud[i], data[i], "Data Mismatch");
            }
        }
    }
}
