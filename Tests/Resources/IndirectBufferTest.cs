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
    public class IndirectBufferTests : RenderDeviceTestBase
    {
        [TestMethod()]
        public void CreateDispatch()
        {
            DispatchIndirectBuffer buffer = new DispatchIndirectBuffer(this.Device);
            Assert.IsNotNull(buffer.ArgumentBuffer, "Argument buffer Is Null");
            Assert.IsNotNull(buffer.WriteBuffer, "Write buffer Is Null");

            buffer.Dispose();
        }

        [TestMethod()]
        public void CreateInstanced()
        {
            InstancedIndirectBuffer buffer = new InstancedIndirectBuffer(this.Device);
            Assert.IsNotNull(buffer.ArgumentBuffer, "Argument buffer Is Null");
            Assert.IsNotNull(buffer.WriteBuffer, "Write buffer Is Null");
            buffer.Dispose();
        }

        [TestMethod()]
        public void CreateIndexed()
        {
            IndexedIndirectBuffer buffer = new IndexedIndirectBuffer(this.Device);
            Assert.IsNotNull(buffer.ArgumentBuffer, "Argument buffer Is Null");
            Assert.IsNotNull(buffer.WriteBuffer, "Write buffer Is Null");
            buffer.Dispose();
        }



    }
}
