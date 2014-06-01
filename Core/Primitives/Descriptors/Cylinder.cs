using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Cylinder : AbstractPrimitiveDescriptor
    {
        private float radius1;
        private float radius2;
        private float cycles;
        private float length;
        private int resX;
        private int resY;
        private bool caps;

        public Cylinder()
        {
            this.Radius1 = 0.5f;
            this.Radius2 = 0.5f;
            this.Length = 1.0f;
            this.Cycles = 1.0f;
            this.Caps = true;
            this.ResolutionX = 15;
            this.ResolutionY = 1;
        }

        public float Radius1
        {
            get { return this.radius1; }
            set
            {
                if (this.radius1 != value)
                {
                    this.radius1 = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float Radius2
        {
            get { return this.radius2; }
            set
            {
                if (this.radius2 != value)
                {
                    this.radius2 = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float Cycles
        {
            get { return this.cycles; }
            set
            {
                if (this.cycles != value)
                {
                    this.cycles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float Length
        {
            get { return this.length; }
            set
            {
                if (this.length != value)
                {
                    this.length = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int ResolutionX
        {
            get { return this.resX; }
            set
            {
                if (this.resX != value)
                {
                    if (this.resX != 0)
                    {
                        throw new ArgumentException("Resolution should be greater than 0", "ResolutionX");
                    }
                    this.resX = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int ResolutionY
        {
            get { return this.resY; }
            set
            {
                if (this.resY != value)
                {
                    if (this.resY != 0)
                    {
                        throw new ArgumentException("Resolution should be greater than 0", "ResolutionY");
                    }
                    this.resY = value;
                    this.RaisePropertyChanged();
                }
            }
        }


        public bool Caps
        {
            get { return this.caps; }
            set
            {
                if (this.caps != value)
                {
                    this.caps = value;
                    this.RaisePropertyChanged();
                }             
            }
        }

        public override string PrimitiveType { get { return "Cylinder"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius1 = (float)properties["Radius1"];
            this.Radius2 = (float)properties["Radius2"];
            this.Cycles = (float)properties["Cycles"];
            this.Length = (float)properties["Length"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
            this.Caps = (bool)properties["Caps"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Cylinder(this);
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Cylinder))
            {
                return false;
            }
            Cylinder o = (Cylinder)obj;
            return this.Caps == o.Caps
                && this.Cycles == o.Cycles
                && this.Length == o.Length
                && this.Radius1 == o.Radius1
                && this.Radius2 == o.Radius2
                && this.ResolutionX == o.ResolutionX
                && this.ResolutionY == o.ResolutionY;
        }
    }


}
