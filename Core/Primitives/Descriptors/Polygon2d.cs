using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace FeralTic.DX11.Geometry
{
    public class Polygon2d : AbstractPrimitiveDescriptor
    {
        private Vector2[] vertices;

        public Polygon2d()
        {
            this.Vertices = new Vector2[]
            {
                new Vector2(-0.5f,0.0f), new Vector2(0.5f,0.0f), new Vector2(0.0f,0.5f)
            };
        }

        public Vector2[] Vertices
        {
            get { return this.vertices; }
            set
            {
                if (value.Length < 3)
                {
                    throw new ArgumentException("Polygon needs at least 3 vertices", "Vertices");
                }
                this.vertices = value;           
            }
        }

        public override string PrimitiveType { get { return "Polygon2d"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Vertices = (Vector2[])properties["Vertices"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Polygon2d(this);
        }
    }
}
