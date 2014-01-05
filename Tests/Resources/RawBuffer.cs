using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX;
using System.Reflection;
using FeralTic.Tests;

namespace FlareTic.Tests
{
    [TestClass]
    public class RawBufferTests : RenderDeviceTestBase
    {
     
        [TestMethod]
        public void CreateRawWriteable()
        {
            RawBufferBindings binding = new RawBufferBindings()
            {
                 AllowIndexBuffer = false,
                 WriteMode = eRawBufferWriteMode.Uav,
                 AllowVertexBuffer = false
            };
            DX11RawBuffer buffer = DX11RawBuffer.CreateWriteable(this.Device, 64, binding);

            Assert.IsNotNull(buffer.Buffer,"Buffer is null");
            Assert.IsNotNull(buffer.ShaderView,"Shader View is null");
            Assert.IsNotNull(buffer.UnorderedView, "UAV is null");

            buffer.Dispose();
        }


        [TestMethod]
        public void CreateRawWriteableV()
        {
            RawBufferBindings binding = new RawBufferBindings()
            {
                AllowIndexBuffer = true,
                WriteMode = eRawBufferWriteMode.None,
                AllowVertexBuffer = false
            };
            DX11RawBuffer buffer = DX11RawBuffer.CreateWriteable(this.Device, 64, binding);

            Assert.IsNotNull(buffer.Buffer, "Buffer is null");
            Assert.IsNotNull(buffer.ShaderView, "Shader View is null");
            Assert.IsNull(buffer.UnorderedView, "UAV is not null");

            buffer.Dispose();
        }

        [TestMethod]
        public void CreateRawWriteableVIS()
        {
            RawBufferBindings binding = new RawBufferBindings()
            {
                AllowIndexBuffer = true,
                WriteMode = eRawBufferWriteMode.StreamOut,
                AllowVertexBuffer = true
            };
            DX11RawBuffer buffer = DX11RawBuffer.CreateWriteable(this.Device, 64, binding);

            Assert.IsNotNull(buffer.Buffer, "Buffer is null");
            Assert.IsNotNull(buffer.ShaderView, "Shader View is null");
            Assert.IsNull(buffer.UnorderedView, "UAV is not null");

            buffer.Dispose();
        }


        [TestMethod]
        public void CreateRawImmutable()
        {
            RawBufferBindings binding = new RawBufferBindings()
            {
                AllowIndexBuffer = false,
                WriteMode = eRawBufferWriteMode.None,
                AllowVertexBuffer = false
            };

            DataStream ds = new DataStream(64, true, true);
            DX11RawBuffer buffer = DX11RawBuffer.CreateImmutable(this.Device,ds,binding);

            Assert.IsNotNull(buffer.Buffer, "Buffer is null");
            Assert.IsNotNull(buffer.ShaderView, "Shader View is null");
            Assert.IsNull(buffer.UnorderedView, "UAV is not null");

            buffer.Dispose();
        }

        [TestMethod]
        public void RawStagingCopy()
        {
            RawBufferBindings binding = new RawBufferBindings()
            {
                AllowIndexBuffer = false,
                WriteMode = eRawBufferWriteMode.None,
                AllowVertexBuffer = true
            };

            DataStream ds = new DataStream(16 * sizeof(uint), true, true);
            for (uint i = 0; i < 16; i++)
            {
                ds.Write<uint>(i);
            }
            ds.Position = 0;

            DX11RawBuffer buffer = DX11RawBuffer.CreateImmutable(this.Device, ds,binding);
            ds.Dispose();

            Assert.IsNotNull(buffer.Buffer, "Immutable Buffer is null");
            Assert.IsNotNull(buffer.ShaderView, "Immutable Shader View is null");
            Assert.IsNull(buffer.UnorderedView, "Immutable UAV is not null");

            DX11RawBuffer staging = DX11RawBuffer.CreateStaging(buffer);

            Assert.IsNotNull(staging.Buffer, "Staging Buffer is null");
            Assert.IsNull(staging.ShaderView, "Staging Shader View is not null");
            Assert.IsNull(staging.UnorderedView, "Immutable UAV is not null");

            this.RenderContext.Context.CopyResource(buffer, staging);

            ds = staging.MapForRead(this.RenderContext);
            for (uint i = 0; i < 16; i++)
            {
                uint d = ds.Read<uint>();
                Assert.AreEqual(i, d, "Invalid Data");
            }

            staging.Dispose();
            buffer.Dispose();
        }



    }
}
