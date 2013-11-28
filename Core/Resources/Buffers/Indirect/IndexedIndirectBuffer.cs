using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class IndexedIndirectBuffer : BaseIndirectBuffer<DrawIndexedInstancedArgs>
    {
        public IndexedIndirectBuffer(DxDevice device, DrawIndexedInstancedArgs args) : base(device, args) { }

        public IndexedIndirectBuffer(DxDevice device) : this(device, new DrawIndexedInstancedArgs(1, 1, 0, 0, 0)) { }

        public void CopyInstanceCount(DeviceContext ctx, UnorderedAccessView uav)
        {
            ctx.CopyStructureCount(this.ArgumentBuffer, 4, uav);
        }

        public void CopyIndicesCount(DeviceContext ctx, UnorderedAccessView uav)
        {
            ctx.CopyStructureCount(this.ArgumentBuffer, 0, uav);
        }
    }
}
