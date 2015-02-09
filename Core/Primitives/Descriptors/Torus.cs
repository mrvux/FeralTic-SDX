using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Torus : AbstractPrimitiveDescriptor
    {
        private float radius;
        private float thick;
        private int resX;
        private int resY;
        private float phasex;
        private float phasey;
        private float rotation;
        private float cy;

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

        public float Thickness
        {
            get { return this.thick; }
            set
            {
                if (this.thick != value)
                {
                    this.thick = value;
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
                    if (value <= 0)
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
                    if (value <= 0)
                    {
                        throw new ArgumentException("Resolution should be greater than 0", "ResolutionY");
                    }
                    this.resY = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float PhaseX
        {
            get { return this.phasex; }
            set
            {
                if (this.phasex != value)
                {
                    this.phasex = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        
        public float PhaseY
        {
            get { return this.phasey; }
            set
            {
                if (this.phasey != value)
                {
                    this.phasey = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public float Rotation
        {
            get { return this.rotation; }
            set
            {
                if (this.rotation != value)
                {
                    this.rotation = value;
                    this.RaisePropertyChanged();
                }
            }
        }


        public float CY
        {
            get { return this.cy; }
            set
            {
                if (this.cy != value)
                {
                    this.cy = value;
                    this.RaisePropertyChanged();
                }
            }
        }


        public Torus()
        {
            this.Radius = 0.5f;
            this.Thickness = 0.1f;
            this.ResolutionX = 15;
            this.ResolutionY = 15;
            this.PhaseX = 1.0f;
            this.PhaseY = 1.0f;
            this.Rotation = 1.0f;
            this.CY = 1.0f;
        }

        public override string PrimitiveType { get { return "Torus"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];

            this.Thickness = (float)properties["Thickness"];

            this.PhaseY = (float)properties["PhaseY"];
            this.PhaseX = (float)properties["PhaseX"];
            this.Rotation = (float)properties["Rotation"];
            this.CY = (float)properties["CY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Torus(this);
        }
    }

}
