﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class Isocahedron : AbstractPrimitiveDescriptor, IEquatable<Isocahedron>
    {
        private Vector3 size;

        public Vector3 Size
        {
            get { return this.size; }
            set
            {
                if (this.size != value)
                {
                    this.size = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Isocahedron()
        {
            this.Size = new Vector3(1, 1, 1);
        }

        public override string PrimitiveType { get { return "Isocahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Isocahedron(this);
        }

        public bool Equals(Isocahedron o)
        {
            return this.Size == o.Size;
        }
    }
}
