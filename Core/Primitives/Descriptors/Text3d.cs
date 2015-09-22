using FeralTic.DX11;
using FeralTic.DX11.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class Text3dPrimitive : AbstractPrimitiveDescriptor
    {
        private string text;
        private string font;
        private float fontsize;
        private float extrudeamount;

        public Text3dPrimitive()
        {
            this.Text = "DX11";
            this.font = "Arial";
            this.fontsize = 32.0f;
            this.extrudeamount = 0.1f;
        }

        public string Text
        {
            get { return this.text; }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public string Font
        {
            get { return this.font; }
            set
            {
                if (this.font != value)
                {
                    this.font = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float FontSize
        {
            get { return this.fontsize; }
            set
            {
                if (this.fontsize != value)
                {
                    this.fontsize = value;
                }
                this.RaisePropertyChanged();
            }
        }

        public float Extrude
        {
            get { return this.extrudeamount; }
            set
            {
                if (this.extrudeamount != value)
                {
                    this.extrudeamount = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public override string PrimitiveType { get { return "Text"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Text = properties["Text"].ToString();
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return null;
            //return device.Primitives.Text(this);
        }
    }
}
