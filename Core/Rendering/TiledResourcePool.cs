using FeralTic.DX11.Resources;
using SharpDX;
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

        private int currentRangeOffset = 0;
        private int aggregatedPageCount = 0;

        public TiledResourcePool(DxDevice device, int initialPageCount = 16)
        {
            /*this.device = device;
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

            this.tilePoolBuffer = new SharpDX.Direct3D11.Buffer(device, bdPool);*/
        }

        public DX11StructuredBuffer PlaceStructuredBuffer(RenderContext context, int elementCount, int stride, eDxBufferMode mode)
        {
            var buffer = DX11StructuredBuffer.CreateTiled(this.device, elementCount, stride, mode);
            int size = buffer.Buffer.Description.SizeInBytes;

            this.AggregateMemory(context, size);
            MapEntireResource(context, buffer.Buffer);
            this.currentRangeOffset = this.aggregatedPageCount;

            return buffer;
        }

        public DX11RenderTarget2D PlaceRenderTarget(RenderContext context, int width, int height, SharpDX.DXGI.Format format)
        {
            var rt = DX11RenderTarget2D.CreateTiled(this.device, width, height, format);

            int fs = SharpDX.DXGI.FormatHelper.SizeOfInBytes(format);
            var tiledSize = this.AlignImage(rt.Width, rt.Height,format);

            int rtSize = tiledSize.Width * tiledSize.Height * fs;

            this.AggregateMemory(context, rtSize);
            MapEntireResource(context, rt.Texture);
            this.currentRangeOffset = this.aggregatedPageCount;

            return rt;
        }

        private void AggregateMemory(RenderContext context, int size)
        {
            this.aggregatedPageCount += this.GetPageCount(size); ;
            ResizeIfNecessary(context, this.aggregatedPageCount * PageSize);
        }

        public void BeginCollect()
        {
            this.currentRangeOffset = 0;
            this.aggregatedPageCount = 0;
        }

        public void EndCollect()
        {
            this.currentRangeOffset = 0;
            this.aggregatedPageCount = 0;
        }

        private void MapEntireResource(RenderContext context, Resource resource)
        {
            var rangeFlags = new TileRangeFlags[] { TileRangeFlags.None };
            context.Context.UpdateTileMappings(resource, 1, new TiledResourceCoordinate[] { }, new TileRegionSize[] { }, this.tilePoolBuffer, 1, rangeFlags, new int[] { this.currentRangeOffset }, new int[] { }, TileMappingFlags.None);
        }

        private int GetPageCount(int memorySize)
        {
            return (memorySize + PageSize - 1) / PageSize;
        }

        private int GetPagedMemoryRequirment(int memorySize)
        {
            return GetPageCount(memorySize) * PageSize;
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

        private Size2 AlignImage(int w, int h, SharpDX.DXGI.Format format)
        {
            int fs = SharpDX.DXGI.FormatHelper.SizeOfInBits(format);

            /*        BC1_Typeless = 70,
        BC1_UNorm = 71,
        BC1_UNorm_SRgb = 72,
        BC2_Typeless = 73,
        BC2_UNorm = 74,
        BC2_UNorm_SRgb = 75,
        BC3_Typeless = 76,
        BC3_UNorm = 77,
        BC3_UNorm_SRgb = 78,
        BC4_Typeless = 79,
        BC4_UNorm = 80,
        BC4_SNorm = 81,*/

            //Those formats are : 512*256
            //other bc formats are 256*256

            Size2 tileSizeForFormat;

            switch (fs)
            {
                case 8:
                    tileSizeForFormat = new Size2(256, 256);
                    break;
                case 16:
                    tileSizeForFormat = new Size2(256, 128);
                    break;
                case 32:
                    tileSizeForFormat = new Size2(128, 128);
                    break;
                case 64:
                    tileSizeForFormat = new Size2(128, 64);
                    break;
                case 128:
                    tileSizeForFormat = new Size2(64, 64);
                    break;
                default:
                    throw new Exception("Unknown tile size for this format");
            }

            int alignedTileWidth = ((w + tileSizeForFormat.Width - 1) / tileSizeForFormat.Width) * tileSizeForFormat.Width;
            int alignedTileHeight = ((h + tileSizeForFormat.Height - 1) / tileSizeForFormat.Height) * tileSizeForFormat.Height;
            return new Size2(alignedTileWidth, alignedTileHeight);
        }
    }
}
