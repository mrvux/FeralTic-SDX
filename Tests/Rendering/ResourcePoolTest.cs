using FeralTic.DX11.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
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

        [TestMethod()]
        public void TestSimilarBuffer()
        {
            DX11StructuredBuffer sb1 = this.Device.ResourcePool.LockStructuredBuffer(16, 16);
            this.Device.ResourcePool.Unlock(sb1);

            DX11StructuredBuffer sb2 = this.Device.ResourcePool.LockStructuredBuffer(16, 16);

            Assert.AreEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestSimilarBufferStride()
        {
            DX11StructuredBuffer sb1 = this.Device.ResourcePool.LockStructuredBuffer(16, 16);
            this.Device.ResourcePool.Unlock(sb1);

            DX11StructuredBuffer sb2 = this.Device.ResourcePool.LockStructuredBuffer<Vector4>(16);

            Assert.AreEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestTwoBuffers()
        {
            DX11StructuredBuffer sb1 = this.Device.ResourcePool.LockStructuredBuffer(16, 16);
            this.Device.ResourcePool.Unlock(sb1);

            DX11StructuredBuffer sb2 = this.Device.ResourcePool.LockStructuredBuffer(16, 25);

            Assert.AreNotEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestBuffersWithFlags()
        {
            DX11StructuredBuffer sb1 = this.Device.ResourcePool.LockStructuredBuffer(16, 16, eDxBufferMode.Default);
            this.Device.ResourcePool.Unlock(sb1);

            DX11StructuredBuffer sb2 = this.Device.ResourcePool.LockStructuredBuffer(16, 16, eDxBufferMode.Append);
            this.Device.ResourcePool.Unlock(sb2);

            DX11StructuredBuffer sb3 = this.Device.ResourcePool.LockStructuredBuffer(16, 16, eDxBufferMode.Counter);
            this.Device.ResourcePool.Unlock(sb2);

            Assert.AreNotEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
            Assert.AreNotEqual(sb2.Buffer.NativePointer, sb3.Buffer.NativePointer);
        }


        [TestMethod()]
        public void TestSimilarVertexBuffers()
        {
            DX11VertexBuffer sb1 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.None);
            this.Device.ResourcePool.Unlock(sb1);

            DX11VertexBuffer sb2 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.None);

            Assert.AreEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestSimilarVertexBufferStride()
        {
            DX11VertexBuffer sb1 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.None);
            this.Device.ResourcePool.Unlock(sb1);

            DX11VertexBuffer sb2 = this.Device.ResourcePool.LockVertexBuffer<Vector4>(20, eVertexBufferWriteMode.None);

            Assert.AreEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestTwoVertexBuffers()
        {
            DX11VertexBuffer sb1 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.None);
            this.Device.ResourcePool.Unlock(sb1);

            DX11VertexBuffer sb2 = this.Device.ResourcePool.LockVertexBuffer(30, 16, eVertexBufferWriteMode.None);

            Assert.AreNotEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestVertexBuffersBuffersWithFlags()
        {
            DX11VertexBuffer sb1 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.None);
            this.Device.ResourcePool.Unlock(sb1);

            DX11VertexBuffer sb2 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.Raw);

            Assert.AreNotEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestSimilarVertexBuffersRaw()
        {
            DX11VertexBuffer sb1 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.Raw);
            this.Device.ResourcePool.Unlock(sb1);

            DX11VertexBuffer sb2 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.Raw);

            Assert.AreEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestSimilarVertexBuffersStreamOut()
        {
            DX11VertexBuffer sb1 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.StreamOut);
            this.Device.ResourcePool.Unlock(sb1);

            DX11VertexBuffer sb2 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.StreamOut);

            Assert.AreEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestVertexBuffersSORaw()
        {
            DX11VertexBuffer sb1 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.StreamOut);
            this.Device.ResourcePool.Unlock(sb1);

            DX11VertexBuffer sb2 = this.Device.ResourcePool.LockVertexBuffer(20, 16, eVertexBufferWriteMode.Raw);

            Assert.AreNotEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

    }
}
