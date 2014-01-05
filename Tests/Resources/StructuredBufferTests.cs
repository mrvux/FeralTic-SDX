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
    public class StructuredBufferTests : RenderDeviceTestBase
    {
        [TestMethod()]
        public void CreateWriteable()
        {
            DX11StructuredBuffer sb = DX11StructuredBuffer.CreateWriteable(this.Device, 16, 16);
            Assert.IsNotNull(sb.Buffer, "Buffer Is Null");
            Assert.IsNotNull(sb.ShaderView, "SRV Is Null");
            Assert.IsNotNull(sb.UnorderedView, "UAV Is Null");
            sb.Dispose();
        }

        [TestMethod()]
        public void CreateWriteablegeneric()
        {
            DX11StructuredBuffer sb = DX11StructuredBuffer.CreateWriteable<Vector4>(this.Device, 16);
            Assert.IsNotNull(sb.Buffer, "Buffer Is Null");
            Assert.IsNotNull(sb.ShaderView, "SRV Is Null");
            Assert.IsNotNull(sb.UnorderedView, "UAV Is Null");
            sb.Dispose();
        }

        [TestMethod()]
        public void CreateAppend()
        {
            DX11StructuredBuffer sb = DX11StructuredBuffer.CreateWriteable(this.Device, 16, 16,eDxBufferMode.Append);
            Assert.IsNotNull(sb.Buffer, "Buffer Is Null");
            Assert.IsNotNull(sb.ShaderView, "SRV Is Null");
            Assert.IsNotNull(sb.UnorderedView, "UAV Is Null");
            sb.Dispose();
        }

        [TestMethod()]
        public void CreateAppendGeneric()
        {
            DX11StructuredBuffer sb = DX11StructuredBuffer.CreateWriteable<Vector4>(this.Device, 16, eDxBufferMode.Append);
            Assert.IsNotNull(sb.Buffer, "Buffer Is Null");
            Assert.IsNotNull(sb.ShaderView, "SRV Is Null");
            Assert.IsNotNull(sb.UnorderedView, "UAV Is Null");
            sb.Dispose();
        }

        [TestMethod()]
        public void CreateCounter()
        {
            DX11StructuredBuffer sb = DX11StructuredBuffer.CreateWriteable(this.Device, 16, 16, eDxBufferMode.Counter);
            Assert.IsNotNull(sb.Buffer, "Buffer Is Null");
            Assert.IsNotNull(sb.ShaderView, "SRV Is Null");
            Assert.IsNotNull(sb.UnorderedView, "UAV Is Null");
            sb.Dispose();
        }

        [TestMethod()]
        public void CreateCounterGeneric()
        {
            DX11StructuredBuffer sb = DX11StructuredBuffer.CreateWriteable<Vector4>(this.Device, 16, eDxBufferMode.Counter);
            Assert.IsNotNull(sb.Buffer, "Buffer Is Null");
            Assert.IsNotNull(sb.ShaderView, "SRV Is Null");
            Assert.IsNotNull(sb.UnorderedView, "UAV Is Null");
            sb.Dispose();
        }

        [TestMethod()]
        public void CreateDynamicStructured()
        {
            DX11StructuredBuffer sb = DX11StructuredBuffer.CreateDynamic<Vector4>(this.Device, 16);
            Assert.IsNotNull(sb.Buffer, "Buffer Is Null");
            Assert.IsNotNull(sb.ShaderView, "SRV Is Null");
            Assert.IsNull(sb.UnorderedView, "UAV Is Not Null");
            sb.Dispose();
        }

        [TestMethod()]
        public void CreateDynamic()
        {
            DX11StructuredBuffer sb = DX11StructuredBuffer.CreateDynamic(this.Device, 16,16);
            Assert.IsNotNull(sb.Buffer, "Buffer Is Null");
            Assert.IsNotNull(sb.ShaderView, "SRV Is Null");
            Assert.IsNull(sb.UnorderedView, "UAV Is Not Null");
            sb.Dispose();
        }

    }
}
