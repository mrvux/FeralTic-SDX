using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class RoundRect : AbstractPrimitiveDescriptor
    {
        private Vector2 innerRadius;
        private float outerRadius;
        private int cornerResolution;
        private bool enableCenter;

        public Vector2 InnerRadius
        {
            get { return this.innerRadius; }
            set
            {
                if (this.innerRadius != value)
                {
                    this.innerRadius = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float OuterRadius
        {
            get { return this.outerRadius; }
            set
            {
                if (this.outerRadius != value)
                {
                    this.outerRadius = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int CornerResolution
        {
            get { return this.cornerResolution; }
            set
            {
                if (this.cornerResolution != value)
                {
                    this.cornerResolution = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public bool EnableCenter
        {
            get { return this.enableCenter; }
            set
            {
                if (this.enableCenter != value)
                {
                    this.enableCenter = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public RoundRect()
        {
            this.InnerRadius = new Vector2(0.35f, 0.35f);
            this.OuterRadius = 0.15f;
            this.EnableCenter = true;
            this.CornerResolution = 20;
        }

        public override string PrimitiveType { get { return "RoundRect"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.InnerRadius = (Vector2)properties["InnerRadius"];
            this.OuterRadius = (float)properties["OuterRadius"];
            this.CornerResolution = (int)properties["CornerResolution"];
            this.EnableCenter = (bool)properties["EnableCenter"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.RoundRect(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoundRect))
            {
                return false;
            }
            RoundRect o = (RoundRect)obj;
            return this.CornerResolution == o.CornerResolution
                && this.EnableCenter == o.EnableCenter
                && this.InnerRadius == o.InnerRadius
                && this.OuterRadius == o.OuterRadius;
        }

    }
}
