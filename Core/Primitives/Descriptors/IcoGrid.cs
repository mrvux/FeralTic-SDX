using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Geometry
{
    public class IcoGrid : AbstractPrimitiveDescriptor
    {
        private Vector2 size;
        private int resX;
        private int resY;

        public Vector2 Size
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

        public IcoGrid()
        {
            this.Size = new Vector2(1, 1);
            this.ResolutionX = 2;
            this.ResolutionY = 2;
        }

        public override string PrimitiveType { get { return "IcoGrid"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.IcoGrid(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IcoGrid))
            {
                return false;
            }
            IcoGrid o = (IcoGrid)obj;
            return this.Size == o.Size
                && this.ResolutionX == o.ResolutionX
                && this.ResolutionY == o.ResolutionY;
        }
    }

}
