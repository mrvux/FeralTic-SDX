using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;


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
    }
}
