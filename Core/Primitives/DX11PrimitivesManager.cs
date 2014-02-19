using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;

namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager : IDisposable
    {
        private RenderDevice device;

        private DX11NullGeometry fulltri;
        private Effect fulltrivs;

        private EffectPass vsonlypass;
        private EffectPass fullscreenpass;

        private DX11IndexedGeometry quad;
        private Effect passtroughVS;
        private InputLayout quadlayout;


        public PrimitivesManager(RenderDevice device)
        {
            this.device = device;
            this.InitializeDelegates();
            Effect e = this.FullTriVS;
        }

        private float Map(float Input, float InMin, float InMax, float OutMin, float OutMax)
        {
            float range = InMax - InMin;
            float normalized = (Input - InMin) / range;
            float output = OutMin + normalized * (OutMax - OutMin);
            float min = Math.Min(OutMin, OutMax);
            float max = Math.Max(OutMin, OutMax);
            return Math.Min(Math.Max(output, min), max);
        }

        public DX11NullGeometry FullScreenTriangle
        {
            get
            {
                if (this.fulltri == null)
                {
                    this.fulltri = new DX11NullGeometry(this.device, 3);
                    this.fulltri.Topology = PrimitiveTopology.TriangleList;
                }
                return this.fulltri;
            }
        }

        public DX11IndexedGeometry FullScreenQuad
        {
            get
            {
                if (this.quad == null)
                {
                    this.quad = this.QuadTextured();
                }

                return this.quad;
            }
        }

        public InputLayout QuadLayout
        {

            get
            {

                if (this.quadlayout == null)
                {

                    this.quadlayout = new InputLayout(device.Device, this.PasstroughVS.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature, this.FullScreenQuad.InputLayout);

                }

                return this.quadlayout;

            }
        }

        public Effect FullTriVS
        {
            get
            {
                this.PrepareFullTri();
                return this.fulltrivs;
            }
        }

        private void PrepareFullTri()
        {
            if (this.fulltrivs == null)
            {
                DX11Effect shader = DX11Effect.CompileFromResource(Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTri.fx");
                this.fulltrivs = new Effect(device.Device, shader.ByteCode);
                this.vsonlypass = this.fulltrivs.GetTechniqueByName("FullScreenTriangleVSOnly").GetPassByIndex(0);
                this.fullscreenpass = this.fulltrivs.GetTechniqueByName("FullScreenTriangle").GetPassByIndex(0);
            }
        }



        public EffectPass FullTriVSPass
        {
            get 
            { 
                return this.vsonlypass; 
            }
        }

        public Effect PasstroughVS
        {
            get
            {

                if (this.passtroughVS == null)
                {

                    DX11Effect shader = DX11Effect.CompileFromResource(Assembly.GetExecutingAssembly(), "FeralTic.Effects.DefaultVS.fx");
                    this.passtroughVS = new Effect(device.Device, shader.ByteCode);
                }
                return this.passtroughVS;
            }
        }

                


        public void ApplyFullTriVS(RenderContext context)
        {
            this.FullScreenTriangle.Bind(context,null);
            this.vsonlypass.Apply(context.Context);
        }

        public void ApplyFullTri(RenderContext context)
        {
            this.FullScreenTriangle.Bind(context,null);
            this.fullscreenpass.Apply(context.Context);
        }

        public void Dispose()
        {
            if (this.fulltrivs != null) { this.fulltrivs.Dispose(); }
        }

    }
}
