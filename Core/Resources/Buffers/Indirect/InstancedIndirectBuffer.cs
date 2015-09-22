using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Resources
{
    public class InstancedIndirectBuffer : BaseIndirectBuffer<DrawInstancedArgs>
    {
        public InstancedIndirectBuffer(DxDevice device, DrawInstancedArgs args) : base(device, args) { }

        public InstancedIndirectBuffer(DxDevice device) : this(device, new DrawInstancedArgs(1, 1, 0, 0)) { }

        public void CopyInstanceCount(DeviceContext ctx, UnorderedAccessView uav)
        {
            ctx.CopyStructureCount(this.ArgumentBuffer, 4, uav);
        }

        public void CopyVertexCount(DeviceContext ctx, UnorderedAccessView uav)
        {
            ctx.CopyStructureCount(this.ArgumentBuffer, 0, uav);
        }

        public void CopyVertexCount(DeviceContext ctx, Buffer buffer, int offset)
        {
            ResourceRegion region = new ResourceRegion(offset, 0, 0, offset + 4, 1, 1); 
            ctx.CopySubresourceRegion(buffer, 0, region, this.ArgumentBuffer, 0, 0, 0, 0);
        }

        public void CopyInstanceCount(DeviceContext ctx, Buffer buffer, int offset)
        {
            ResourceRegion region = new ResourceRegion(offset, 0, 0, offset + 4, 1, 1);
            ctx.CopySubresourceRegion(buffer, 0, region, this.ArgumentBuffer, 0, 4, 0, 0);
        }
    }
}
