using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.Direct3D;


namespace FeralTic.DX11.Resources
{
    public abstract class DX11BaseGeometry : IDX11Geometry
    {
        protected DxDevice device;

        public DX11BaseGeometry(DxDevice device, bool resourceowner = true)
        {
            this.device = device;
        }

        internal DX11BaseGeometry()
        {

        }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public virtual InputElement[] InputLayout { get; set; }


        public PrimitiveTopology Topology
        {
            get; set;
        }

        public bool ValidateLayout(EffectPass pass, out InputLayout layout)
        {
            layout = null;
            try
            {
                layout = new InputLayout(device.Device, pass.Description.Signature, this.InputLayout);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BoundingBox BoundingBox { get; set; }

        public bool HasBoundingBox { get; set; }

        public abstract void Draw(DX11RenderContext context);

        public abstract void Bind(DX11RenderContext context, InputLayout layout);

        public abstract void Dispose();

        public abstract IDX11Geometry ShallowCopy();

        public object Tag { get; set; }

        public string PrimitiveType { get; set; }
    }
}
