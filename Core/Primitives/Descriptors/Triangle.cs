using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Triangle : AbstractPrimitiveDescriptor
    {
        private Vector3 p1;
        private Vector3 p2;
        private Vector3 p3;

        public Vector3 P1
        {
            get { return this.p1; }
            set
            {
                if (this.p1 != value)
                {
                    this.p1 = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Vector3 P2
        {
            get { return this.p2; }
            set
            {
                if (this.p2 != value)
                {
                    this.p2 = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Vector3 P3
        {
            get { return this.p3; }
            set
            {
                if (this.p3 != value)
                {
                    this.p3 = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Triangle()
        {
            this.p1 = new Vector3(-0.5f, 0.0f, 0.0f);
            this.p2 = new Vector3(0.5f, 0.0f, 0.0f);
            this.p3 = new Vector3(0.0f, 0.5f, 0.0f);
        }

        public override string PrimitiveType { get { return "Triangle"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.p1 = (Vector3)properties["P1"];
            this.p2 = (Vector3)properties["P2"];
            this.p3 = (Vector3)properties["P3"];
            this.RaisePropertyChanged();
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Triangle(this);
        }
    }
}
