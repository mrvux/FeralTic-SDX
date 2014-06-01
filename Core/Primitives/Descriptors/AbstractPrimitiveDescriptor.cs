using FeralTic.DX11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public abstract class AbstractPrimitiveDescriptor
    {
        public abstract string PrimitiveType { get; }
        public abstract void Initialize(Dictionary<string, object> properties);
        public abstract IDxGeometry GetGeometry(RenderDevice device);

        protected void RaisePropertyChanged()
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new EventArgs());
            }
        }

        public event EventHandler PropertyChanged;
    }

}
