using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class TextPrimitive : AbstractPrimitiveDescriptor
    {
        public TextPrimitive()
        {
            this.Text = "DX11";
        }

        public string Text { get; set; }

        public override string PrimitiveType { get { return "Text"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Text = properties["Text"].ToString();
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Text(this);
        }
    }
}
