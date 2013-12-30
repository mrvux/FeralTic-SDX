using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Just a no geometry as specified. So will work in cases where 
    /// only SV_VertexId or SV_InstanceId is required
    /// </summary>
    public class DX11NullGeometry : DX11BaseGeometry
    {
        private IDX11GeometryDrawer<DX11NullGeometry> drawer;


        public IDX11GeometryDrawer<DX11NullGeometry> Drawer { get { return this.drawer; } }


        public DX11NullGeometry(DxDevice device)
            : base(device)
        {
            this.drawer = new DX11NullDrawer();
            this.drawer.Assign(this);

            this.InputLayout = new InputElement[0];
            this.Topology = PrimitiveTopology.PointList;
        }

        public DX11NullGeometry(DxDevice device, int VertexCount)
            : base(device)
        {
            DX11NullDrawer d = new DX11NullDrawer();
            d.VertexCount = VertexCount;
           
            this.drawer = d;
            this.drawer.Assign(this);
            

            this.InputLayout = new InputElement[0];
            this.Topology = PrimitiveTopology.PointList;
        }

        public DX11NullGeometry(DxDevice device, IDX11GeometryDrawer<DX11NullGeometry> drawer)
            : base(device)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
            this.InputLayout = new InputElement[0];
            this.Topology = PrimitiveTopology.PointList;
        }

        public void AssignDrawer(IDX11GeometryDrawer<DX11NullGeometry> drawer)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
        }

        public override void Bind(RenderContext ctx, InputLayout layout)
        {
            ctx.Context.InputAssembler.PrimitiveTopology = this.Topology;
            drawer.PrepareInputAssembler(ctx, layout);
        }

        public override void Draw(RenderContext ctx)
        {
            this.drawer.Draw(ctx);
        }

        public override void Dispose()
        {

        }

        public DX11NullGeometry(DX11NullGeometry owner)
        {
            this.BoundingBox = owner.BoundingBox;
            this.AssignDrawer(owner.Drawer);
        }

        public override IDxGeometry ShallowCopy()
        {
            return new DX11NullGeometry(this);
        }
    }
}
