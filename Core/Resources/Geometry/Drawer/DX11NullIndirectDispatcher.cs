using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using FeralTic.DX11;

namespace FeralTic.Resources.Geometry
{
    public class DX11NullIndirectDispatcher : IDX11GeometryDrawer<DX11NullGeometry>
    {
        private DX11NullGeometry geom;
        public DispatchIndirectBuffer IndirectArgs { get; set; }


        public void PrepareInputAssembler(RenderContext ctx, InputLayout layout)
        {

        }

        public void Draw(RenderContext ctx)
        {
            //ctx.ComputeShader.
            ctx.Context.DispatchIndirect(this.IndirectArgs.ArgumentBuffer, 0);
        }

        public void Dispose()
        {
           
        }

        public void Assign(DX11NullGeometry geometry)
        {
            this.geom = geometry;
        }
    }
}
