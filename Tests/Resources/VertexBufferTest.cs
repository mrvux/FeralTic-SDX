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
    public class VertexBufferTests : RenderDeviceTestBase
    {
        [TestMethod()]
        public void CreateImmutable()
        {
            Vector3[] v = new Vector3[16];
            DX11VertexBuffer vbo = DX11VertexBuffer.CreateImmutable<Vector3>(Device, v);

            Assert.IsNotNull(vbo.Buffer, "Buffer Is Null");

            vbo.Dispose();
        }

        /*[TestMethod()]
        public void CreateWriteableRawAndSO()
        {
            Vector3[] v = new Vector3[16];
            DX11VertexBuffer vbo = DX11VertexBuffer.CreateWriteable(Device, 16, 16, true, true);

            Assert.IsNotNull(vbo.Buffer, "Buffer Is Null");
            vbo.Dispose();
        }*/

        [TestMethod()]
        public void CreateWriteableRaw()
        {
            Vector3[] v = new Vector3[16];
            DX11VertexBuffer vbo = DX11VertexBuffer.CreateWriteable(Device, 16, 16, eVertexBufferWriteMode.Raw);

            Assert.IsNotNull(vbo.Buffer, "Buffer Is Null");
            vbo.Dispose();
        }

        [TestMethod()]
        public void CreateWriteableSO()
        {
            Vector3[] v = new Vector3[16];
            DX11VertexBuffer vbo = DX11VertexBuffer.CreateWriteable(Device, 16, 16, eVertexBufferWriteMode.StreamOut);

            Assert.IsNotNull(vbo.Buffer, "Buffer Is Null");
            vbo.Dispose();
        }


    }
}
