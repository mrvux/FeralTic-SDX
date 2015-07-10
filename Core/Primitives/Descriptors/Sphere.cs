using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Sphere : AbstractPrimitiveDescriptor, IEquatable<Sphere>
    {
        private float radius;
        private float cyclesx;
        private float cyclesy;
        private int resX;
        private int resY;

        public Sphere()
        {
            this.Radius = 0.5f;
            this.CyclesX = 1.0f;
            this.CyclesY = 1.0f;
            this.ResX = 15;
            this.ResY = 15;
        }

        public float Radius
        {
            get { return this.radius; }
            set
            {
                if (this.radius != value)
                {
                    this.radius = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float CyclesX
        {
            get { return this.cyclesx; }
            set
            {
                if (this.cyclesx != value)
                {
                    this.cyclesx = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float CyclesY
        {
            get { return this.cyclesy; }
            set
            {
                if (this.cyclesy != value)
                {
                    this.cyclesy = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int ResX
        {
            get { return this.resX; }
            set
            {
                if (this.resX != value)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentException("Resolution should be greater than 0", "ResolutionX");
                    }
                    this.resX = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int ResY
        {
            get { return this.resY; }
            set
            {
                if (this.resY != value)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentException("Resolution should be greater than 0", "ResolutionY");
                    }
                    this.resY = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public override string PrimitiveType { get { return "Sphere"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.ResX = (int)properties["ResX"];
            this.ResY = (int)properties["ResY"];
            this.CyclesX = (float)properties["CyclesX"];
            this.CyclesY = (float)properties["CyclesY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Sphere(this);
        }

        public bool Equals(Sphere o)
        {
            return this.CyclesX == o.CyclesX
                && this.CyclesY == o.CyclesY
                && this.Radius == o.Radius
                && this.ResX == o.ResX
                && this.ResY == o.ResY;
        }
    }

}
