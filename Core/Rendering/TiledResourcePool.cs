using FeralTic.DX11.Resources;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace FeralTic.DX11
{
    public class TiledResourcePool : IDisposable
    {
        private readonly DxDevice device;
        private Buffer tilePoolBuffer;

        private int pageCount;

        private const int PageSize = 65536;

        public TiledResourcePool(DxDevice device, int initialPageCount = 8)
        {
            this.device = device;
            this.pageCount = initialPageCount;
            var memSize = initialPageCount * 65536;

            BufferDescription bdPool = new BufferDescription()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.TilePool,
                SizeInBytes = memSize,
                Usage = ResourceUsage.Default
            };

            this.tilePoolBuffer = new SharpDX.Direct3D11.Buffer(device, bdPool);
        }

        public DX11StructuredBuffer PlaceStructuredBuffer(RenderContext context, int elementCount, int stride, eDxBufferMode mode)
        {
            var buffer = DX11StructuredBuffer.CreateTiled(this.device, elementCount, stride, mode);
            int size = buffer.Buffer.Description.SizeInBytes;

            ResizeIfNecessary(context, size);
            MapEntireResource(context, buffer.Buffer);
            return buffer;
        }

        public DX11RenderTarget2D PlaceRenderTarget(RenderContext context, int width, int height, SharpDX.DXGI.Format format)
        {
            var rt = DX11RenderTarget2D.CreateTiled(this.device, width, height, format);

            int fs = SharpDX.DXGI.FormatHelper.SizeOfInBytes(format);
            int rtSize = rt.Width * rt.Height * fs;

            ResizeIfNecessary(context, rtSize);
            MapEntireResource(context, rt.Texture);
            return rt;
        }

        private void MapEntireResource(RenderContext context, Resource resource)
        {
            var rangeFlags = new TileRangeFlags[] { TileRangeFlags.None };
            context.Context.UpdateTileMappings(resource, 1, new TiledResourceCoordinate[] { }, new TileRegionSize[] { }, this.tilePoolBuffer, 1, rangeFlags, new int[] { 0 }, new int[] { }, TileMappingFlags.None);
        }

        private int GetPageCount(int memorySize)
        {
            return (memorySize + PageSize - 1) / PageSize;
        }

        public int TotalMemorySize
        {
            get { return this.pageCount * PageSize; }
        }

        private void ResizeIfNecessary(RenderContext context, int requiredMemorySize)
        {
            if (this.TotalMemorySize < requiredMemorySize)
            {
                this.Resize(context, requiredMemorySize);
            }
        }

        private void Resize(RenderContext context, int requiredMemorySize)
        {
            int newPageCount = this.GetPageCount(requiredMemorySize);

            context.Context.ResizeTilePool(this.tilePoolBuffer, newPageCount * PageSize);

            this.pageCount = newPageCount;
        }

        public void Dispose()
        {
            this.tilePoolBuffer.Dispose();
        }
    }
}
