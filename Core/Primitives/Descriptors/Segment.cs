using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Segment : AbstractPrimitiveDescriptor
    {
        private float phase;
        private float cycles;
        private float innerradius;
        private bool flat;
        private int resolution;

        public float Phase
        {
            get { return this.phase; }
            set
            {
                if (this.phase != value)
                {
                    this.phase = value;
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

        public float InnerRadius
        {
            get { return this.innerradius; }
            set
            {
                if (this.innerradius != value)
                {
                    this.innerradius = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public bool Flat
        {
            get { return this.flat; }
            set
            {
                if (this.flat != value)
                {
                    this.flat = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int Resolution
        {
            get { return this.resolution; }
            set
            {
                if (this.resolution != value)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentException("Resolution should be greater than 0", "Resolution");
                    }
                    this.resolution = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Segment()
        {
            this.Phase = 0.0f;
            this.Cycles = 1.0f;
            this.InnerRadius = 0.0f;
            this.Flat = true;
            this.Resolution = 20;
        }

        public override string PrimitiveType { get { return "Segment"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Phase = (float)properties["Phase"];
            this.Cycles = (float)properties["Cycles"];
            this.InnerRadius = (float)properties["InnerRadius"];
            this.Resolution = (int)properties["Resolution"];
            this.Flat = (bool)properties["Flat"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Segment(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Segment))
            {
                return false;
            }
            Segment o = (Segment)obj;
            return this.Cycles == o.Cycles
                && this.Flat == o.Flat
                && this.InnerRadius == o.InnerRadius
                && this.Phase == o.Phase
                && this.Resolution == o.Resolution;
        }
    }
}
