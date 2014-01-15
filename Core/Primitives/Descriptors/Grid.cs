using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Grid : AbstractPrimitiveDescriptor
    {
        public Grid()
        {
            this.Size = new Vector2(1, 1);
            this.ResolutionX = 2;
            this.ResolutionY = 2;
        }

        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }

        public override string PrimitiveType { get { return "Grid"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Grid(this);
        }
    }
}
