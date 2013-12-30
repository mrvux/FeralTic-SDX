using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.GeometryFX
{
    public class GeometryFXSettings : IViewProjectionSettings
    {
        public RenderDevice Device { get; set; }

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Matrix ViewProjection { get; set; }

        public bool AsAuto { get; set; }
        public bool UseMaxElements { get; set; }
        public int MaxElements { get; set; }

        private int outputvertexcount;
        private IDxGeometry outputgeometry;
        private DX11RawBuffer rawbuffer;
        private StreamOutputBufferBinding[] binding;

        private InputElement[] outputlayout;
        private int outputvertexsize;

        public void AssignPass(EffectPass pass)
        {
            this.outputlayout = pass.GetStreamOutputLayout(out this.outputvertexsize);
        }

        public void AssignGeometry(IDxGeometry geometry)
        {
            this.outputvertexcount = this.GetVertexCount(geometry);
            int buffersize = this.outputvertexcount * this.outputvertexsize;

            if (rawbuffer != null)
            {
                if (buffersize != this.rawbuffer.Size)
                {
                    rawbuffer.Dispose(); rawbuffer = null;
                }
            }

            if (rawbuffer == null)
            {
                rawbuffer = DX11RawBuffer.CreateWriteable(this.Device, buffersize, new RawBufferBindings() { AllowVertexBuffer = true, AllowStreamOut = true });
            }
            this.binding = new StreamOutputBufferBinding[] { new StreamOutputBufferBinding(this.rawbuffer.Buffer, 0) };
            this.outputgeometry = this.PrepareGeometry(geometry);
        }

        private int GetVertexCount(IDxGeometry geometry)
        {
            if (this.UseMaxElements) { return this.MaxElements; }

            if (geometry is DX11VertexGeometry)
            {
                DX11VertexGeometry vd = (DX11VertexGeometry)geometry;
                return vd.VerticesCount;
            }
            if (geometry is DX11IndexedGeometry)
            {
                DX11IndexedGeometry id = (DX11IndexedGeometry)geometry;
                return id.VertexBuffer.VerticesCount;
            }
            if (geometry is DX11NullGeometry)
            {
                return this.MaxElements;
            }

            throw new NotSupportedException("Can't set Vertices count from provided geometry");
        }

        private IDxGeometry PrepareGeometry(IDxGeometry geometry)
        {
            if (geometry is DX11VertexGeometry)
            {
                DX11VertexGeometry vd = (DX11VertexGeometry)geometry.ShallowCopy();
                vd.VertexBuffer = this.rawbuffer.Buffer;
                vd.VertexSize = this.outputvertexsize;
                vd.VerticesCount = this.outputvertexcount;
                vd.InputLayout = this.outputlayout;
                if (this.AsAuto)
                {
                    DX11VertexAutoDrawer auto = new DX11VertexAutoDrawer();
                    vd.AssignDrawer(auto);
                }
                return vd;
            }
            if (geometry is DX11IndexedGeometry)
            {
                if (this.AsAuto)
                {
                    DX11VertexGeometry vd = new DX11VertexGeometry(this.Device);
                    vd.VertexBuffer = this.rawbuffer.Buffer;
                    vd.VertexSize = this.outputvertexsize;
                    vd.VerticesCount = this.outputvertexcount;
                    vd.InputLayout = this.outputlayout;
                    DX11VertexAutoDrawer auto = new DX11VertexAutoDrawer();
                    vd.AssignDrawer(auto);
                    return vd;
                }
                else
                {
                    //Replace the vertexbuffer
                    DX11IndexedGeometry id = (DX11IndexedGeometry)geometry.ShallowCopy();
                    id.InputLayout = this.outputlayout;
                    id.VertexBuffer = DX11VertexBuffer.CreateFromRawBuffer(this.Device, this.outputvertexcount, this.outputvertexsize, this.rawbuffer);
                    return id;
                }
            }
            if (geometry is DX11NullGeometry)
            {
                DX11VertexGeometry vd = new DX11VertexGeometry(this.Device);
                vd.VertexBuffer = this.rawbuffer.Buffer;
                vd.VertexSize = this.outputvertexsize;
                vd.VerticesCount = this.outputvertexcount;
                vd.InputLayout = this.outputlayout;
                if (this.AsAuto)
                {
                    DX11VertexAutoDrawer auto = new DX11VertexAutoDrawer();
                    vd.AssignDrawer(auto);
                }
                return vd;
            }

            throw new NotSupportedException("Can't prepare geometry from provided geometry");
        }

        public void BindOutput(RenderContext context)
        {
            context.Context.StreamOutput.SetTargets(this.binding);
        }
    }
}
