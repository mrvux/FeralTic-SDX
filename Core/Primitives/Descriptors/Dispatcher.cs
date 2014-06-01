using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Dispatcher : AbstractPrimitiveDescriptor
    {
        private int tx;
        private int ty;
        private int tz;

        public int ThreadCountX
        {
            get { return this.tx; }
            set
            {
                if (tx != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Thread Count must be positive or 0", "ThreadCountX");
                    }
                    this.tx = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int ThreadCountY
        {
            get { return this.ty; }
            set
            {
                if (ty != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Thread Count must be positive or 0", "ThreadCountY");
                    }
                    this.ty = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int ThreadCountZ
        {
            get { return this.tz; }
            set
            {
                if (tz != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Thread Count must be positive or 0", "ThreadCountZ");
                    }
                    this.tz = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Dispatcher()
        {
            this.ThreadCountX = 1;
            this.ThreadCountY = 1;
            this.ThreadCountZ = 1;
        }

        public override string PrimitiveType { get { return "Dispatcher"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Dispatcher(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Dispatcher))
            {
                return false;
            }
            Dispatcher o = (Dispatcher)obj;
            return this.ThreadCountX == o.ThreadCountX
                && this.ThreadCountY == o.ThreadCountY
                && this.ThreadCountZ == o.ThreadCountZ;
        }
    }
}
