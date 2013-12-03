using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Utils;
using FeralTic.DX11.Resources;

namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager
    {
        private delegate AbstractPrimitiveDescriptor CreatePrimitiveDelegate();

        private Dictionary<string, CreatePrimitiveDelegate> primitivecreator = new Dictionary<string,CreatePrimitiveDelegate>();
        private Dictionary<string, Type> primitivetypes = new Dictionary<string, Type>();

        private void Register<T>(string key) where T : AbstractPrimitiveDescriptor, new()
        {
            this.primitivecreator.Add(key, () => new T());
            this.primitivetypes.Add(key,typeof(T));
        }

        private void InitializeDelegates()
        {
            Register<Box>("Box");
            Register<Cylinder>("Cylinder");
            Register<Grid>("Grid");
            Register<IcoGrid>("IcoGrid");
            Register<Isocahedron>("Isocahedron");
            Register<Octahedron>("Octahedron");
            Register<Tetrahedron>("Tetrahedron");
            Register<Quad>("Quad");
            Register<RoundRect>("RoundRect");
            Register<Segment>("Segment");
            Register<SegmentZ>("SegmentZ");
            Register<Sphere>("Sphere");
            Register<Torus>("Torus");
        }

        public Type GetDescriptorType(string ptype)
        {
            if (this.primitivetypes.ContainsKey(ptype))
            {
                return this.primitivetypes[ptype];
            }
            else
            {
                throw new Exception("Unknown Primitive Type");
            }
        }

        public IDX11Geometry GetByPrimitiveType(string ptype, Dictionary<string,object> properties)
        {
            if (this.primitivecreator.ContainsKey(ptype))
            {
                AbstractPrimitiveDescriptor descriptor = this.primitivecreator[ptype]();
                descriptor.Initialize(properties);
                return descriptor.GetGeometry(this.device);
            }
            else
            {
                throw new Exception("Unknown Primitive Type");
            }
        }
    }

}
