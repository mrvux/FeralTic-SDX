using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.DXGI;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public class StructuredBufferPool : ResourcePool<DX11StructuredBuffer>
    {
        public StructuredBufferPool(DxDevice device)
            : base(device)
        {

        }

        public ResourcePoolEntry<DX11StructuredBuffer> Lock(int stride, int numelements,eDxBufferMode mode = eDxBufferMode.Default)
        {
            foreach (ResourcePoolEntry<DX11StructuredBuffer> entry in this.pool)
            {
                DX11StructuredBuffer tr = entry.Element;

                if (!entry.IsLocked && tr.ElementCount == numelements && tr.Stride == stride && tr.BufferMode == mode)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11StructuredBuffer res = DX11StructuredBuffer.CreateWriteable(device, numelements, stride, mode);
            ResourcePoolEntry<DX11StructuredBuffer> newentry = new ResourcePoolEntry<DX11StructuredBuffer>(res);
            this.pool.Add(newentry);
            return newentry;
        }
    }
    
    public class RenderTargetPool : ResourcePool<DX11RenderTarget2D>
    {
        public RenderTargetPool(DxDevice device)
            : base(device)
        {

        }

        public ResourcePoolEntry<DX11RenderTarget2D> Lock(int w, int h, Format format, SampleDescription sd, bool genMM = false, int mmLevels = 1)
        {
            foreach (ResourcePoolEntry<DX11RenderTarget2D> entry in this.pool)
            {
                DX11RenderTarget2D tr = entry.Element;

                if (!entry.IsLocked && tr.Width == w && tr.Format == format && tr.Height == h
                    && tr.Texture.Description.SampleDescription.Count == sd.Count
                    && tr.Texture.Description.SampleDescription.Quality == sd.Quality)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11RenderTarget2D res = new DX11RenderTarget2D(this.device, w, h,sd, format, genMM, mmLevels);

            ResourcePoolEntry<DX11RenderTarget2D> newentry = new ResourcePoolEntry<DX11RenderTarget2D>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }

    public class DepthStencilPool : ResourcePool<DX11DepthStencil>
    {
        public DepthStencilPool(DxDevice device)
            : base(device)
        {

        }

        public ResourcePoolEntry<DX11DepthStencil> Lock(int w, int h, eDepthFormat format, SampleDescription sd)
        {
            foreach (ResourcePoolEntry<DX11DepthStencil> entry in this.pool)
            {
                DX11DepthStencil tr = entry.Element;

                if (!entry.IsLocked && tr.Width == w && tr.DepthFormat == format && tr.Height == h
                    && tr.Texture.Description.SampleDescription.Count == sd.Count
                    && tr.Texture.Description.SampleDescription.Quality == sd.Quality)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11DepthStencil res = new DX11DepthStencil(this.device, w, h, sd, format);

            ResourcePoolEntry<DX11DepthStencil> newentry = new ResourcePoolEntry<DX11DepthStencil>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }

    public class VertexBufferPool : ResourcePool<DX11VertexBuffer>
    {
        public VertexBufferPool(DxDevice device)
            : base(device)
        {

        }

        public ResourcePoolEntry<DX11VertexBuffer> Lock(int verticescount, int vertexsize, eVertexBufferWriteMode mode)
        {
            //We can lock any buffer of the right size
            int totalsize = vertexsize * verticescount;

            foreach (ResourcePoolEntry<DX11VertexBuffer> entry in this.pool)
            {
                DX11VertexBuffer tr = entry.Element;


                if (!entry.IsLocked && totalsize == tr.TotalSize && mode == tr.WriteMode)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11VertexBuffer res = DX11VertexBuffer.CreateWriteable(this.device, verticescount, vertexsize, mode);

            ResourcePoolEntry<DX11VertexBuffer> newentry = new ResourcePoolEntry<DX11VertexBuffer>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }

    public class VolumeTexturePool : ResourcePool<DX11RenderTexture3D>
    {
        public VolumeTexturePool(DxDevice device)
            : base(device)
        {

        }

        public ResourcePoolEntry<DX11RenderTexture3D> Lock(int w, int h, int d,Format format)
        {
            foreach (ResourcePoolEntry<DX11RenderTexture3D> entry in this.pool)
            {
                DX11RenderTexture3D tr = entry.Element;

                if (!entry.IsLocked && tr.Width == w && tr.Format == format && tr.Height == h && tr.Depth == d)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11RenderTexture3D res = new DX11RenderTexture3D(this.device, w, h, d, format);

            ResourcePoolEntry<DX11RenderTexture3D> newentry = new ResourcePoolEntry<DX11RenderTexture3D>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }
}
