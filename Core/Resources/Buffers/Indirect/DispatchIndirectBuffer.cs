using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;


namespace FeralTic.DX11.Resources
{
    public class DispatchIndirectBuffer : BaseIndirectBuffer<DispatchArgs>
    {
        public DispatchIndirectBuffer(DxDevice device, DispatchArgs args) : base(device, args) { }

        public DispatchIndirectBuffer(DxDevice device) : this(device, new DispatchArgs(1, 1, 1)) { }

    }
}
