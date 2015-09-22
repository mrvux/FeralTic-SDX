using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class IcoSphere : AbstractPrimitiveDescriptor, IEquatable<IcoSphere>
    {
        private float radius;
        private int subdiv;

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

        public int SubDivisions
        {
            get { return this.subdiv; }
            set
            {
                if (this.subdiv != value)
                {
                    this.subdiv = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public IcoSphere()
        {
            this.Radius = 0.5f;
            this.SubDivisions = 1;
        }

        public override string PrimitiveType { get { return "IcoSphere"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.SubDivisions = (int)properties["SubDivisions"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.IcoSphere(this);
        }

        public bool Equals(IcoSphere o)
        {
            return this.Radius == o.Radius
                && this.SubDivisions == o.SubDivisions;
        }
    }

}
