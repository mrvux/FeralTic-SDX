using FeralTic.DX11;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Buffer = SharpDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Constructs a region within a structured buffer
    /// </summary>
    public unsafe class DX11StructuredBufferRegion : IDxBuffer, IDxShaderResource, IDxUnorderedResource, IDisposable
    {
        public Buffer Buffer { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public UnorderedAccessView UnorderedView { get; protected set; }

        public int ElementCount { get; protected set; }
        public int Stride { get; protected set; }

        public DX11StructuredBufferRegion(DxDevice device, DX11StructuredBuffer parentBuffer, int StartOffset, int ElementCount)
        {
            this.Buffer = parentBuffer.Buffer;
            this.ElementCount = ElementCount;
            this.Stride = parentBuffer.Stride;

            UnorderedAccessViewDescription uavd = new UnorderedAccessViewDescription()
            {
                Format = SharpDX.DXGI.Format.Unknown,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Buffer = new UnorderedAccessViewDescription.BufferResource()
                {
                    ElementCount = this.ElementCount,
                    Flags = parentBuffer.UnorderedView.Description.Buffer.Flags,
                    FirstElement = StartOffset
                }
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Format = SharpDX.DXGI.Format.Unknown,
                Dimension = ShaderResourceViewDimension.Buffer,
                BufferEx = new ShaderResourceViewDescription.ExtendedBufferResource()
                {
                    ElementCount = this.ElementCount,
                    FirstElement = StartOffset
                }
            };

            this.ShaderView = new ShaderResourceView(device, parentBuffer.Buffer, srvd);
            this.UnorderedView = new UnorderedAccessView(device, parentBuffer.Buffer, uavd);
        }

        public void Dispose()
        {
            this.ShaderView.Dispose();
            this.UnorderedView.Dispose();
        }
    }
}
