using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;
using SharpDX;

namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager : IDisposable
    {
        private RenderDevice device;

        private DX11NullGeometry fulltri;
        private DX11IndexedGeometry quad;
        
        private InputLayout quadlayout;

        private VertexShader VSQuad;
        private VertexShader VSTri;

        private PixelShader PSPass;
        private PixelShader PSGray;
        private PixelShader PSLuma;
        private PixelShader PSAlpha;
        private ConstantBuffer<float> cbLuma;

        private SamplerState LinearSampler;


        public PrimitivesManager(RenderDevice device)
        {
            this.device = device;
            this.quad = this.QuadTextured();
            this.fulltri = new DX11NullGeometry(device, 3);
            this.fulltri.Topology = PrimitiveTopology.TriangleList;
            this.LinearSampler = device.SamplerStates.LinearClamp;
            this.cbLuma = new ConstantBuffer<float>(device, true);

            this.InitializeDelegates();
            this.PrepareBasicShaders();
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
                return this.fulltri;
            }
        }

        public DX11IndexedGeometry FullScreenQuad
        {
            get
            {
                return this.quad;
            }
        }

        public InputLayout QuadLayout
        {
            get
            {
                return this.quadlayout;
            }
        }

        private void PrepareBasicShaders()
        {
            this.VSTri = ShaderCompiler.CompileFromResource<VertexShader>(this.device, Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTri.fx", "VSFullTri");
            this.PSPass = ShaderCompiler.CompileFromResource<PixelShader>(this.device, Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTri.fx", "PS");
            this.PSGray = ShaderCompiler.CompileFromResource<PixelShader>(this.device, Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTri.fx", "PSGray");
            this.PSLuma = ShaderCompiler.CompileFromResource<PixelShader>(this.device, Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTri.fx", "PSLuma");
            this.PSAlpha = ShaderCompiler.CompileFromResource<PixelShader>(this.device, Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTri.fx", "PSAlpha");

            ShaderSignature quadsignature;
            this.VSQuad = ShaderCompiler.CompileFromResource(this.device, Assembly.GetExecutingAssembly(), "FeralTic.Effects.DefaultVS.fx", "VS", out quadsignature);
            this.quadlayout = new InputLayout(device, quadsignature, this.quad.InputLayout);
        }

        public void ApplyFullTriVS(RenderContext context)
        {
            this.FullScreenTriangle.Bind(context,null);
            context.Context.VertexShader.Set(this.VSTri);
        }

        public void ApplyPixelPasstrough(RenderContext context)
        {
            context.Context.PixelShader.Set(this.PSPass);
        }

        public void ApplyPixelGray(RenderContext context)
        {
            context.Context.PixelShader.Set(this.PSPass);
        }

        public void ApplyFullTri(RenderContext context, IDxTexture2D texture)
        {
            this.FullScreenTriangle.Bind(context,null);

            context.Context.VertexShader.Set(this.VSTri);
            context.Context.PixelShader.Set(this.PSPass);
            context.Context.PixelShader.SetShaderResource(0, texture == null ? null : texture.ShaderView);
            context.Context.PixelShader.SetSampler(0, this.LinearSampler);
        }

        public void ApplyFullTri(RenderContext context, ShaderResourceView resourceView)
        {
            this.FullScreenTriangle.Bind(context, null);

            context.Context.VertexShader.Set(this.VSTri);
            context.Context.PixelShader.Set(this.PSPass);
            context.Context.PixelShader.SetShaderResource(0, resourceView);
            context.Context.PixelShader.SetSampler(0, this.LinearSampler);
        }

        public void ApplyFullTriGray(RenderContext context, IDxTexture2D texture)
        {
            this.FullScreenTriangle.Bind(context, null);
            context.Context.VertexShader.Set(this.VSTri);
            context.Context.PixelShader.Set(this.PSGray);
            context.Context.PixelShader.SetShaderResource(0, texture == null ? null : texture.ShaderView);
            context.Context.PixelShader.SetSampler(0, this.LinearSampler);
        }


        public void ApplyFullTriAlpha(RenderContext context, IDxTexture2D texture, float alpha)
        {
            this.cbLuma.Update(context, ref alpha);
            this.FullScreenTriangle.Bind(context, null);
            context.Context.VertexShader.Set(this.VSTri);
            context.Context.PixelShader.Set(this.PSAlpha);
            context.Context.PixelShader.SetShaderResource(0,texture == null ? null : texture.ShaderView);
            context.Context.PixelShader.SetSampler(0, this.LinearSampler);
        }

        public void ApplyFullTriLuma(RenderContext context, IDxTexture2D texture, float luma)
        {
            this.cbLuma.Update(context, ref luma);
            this.FullScreenTriangle.Bind(context, null);
            context.Context.VertexShader.Set(this.VSTri);
            context.Context.PixelShader.Set(this.PSLuma);
            context.Context.PixelShader.SetShaderResource(0, texture == null ? null : texture.ShaderView);
            context.Context.PixelShader.SetSampler(0, this.LinearSampler);
            context.Context.PixelShader.SetConstantBuffer(0, this.cbLuma.Buffer);
        }

        private DX11IndexedGeometry FromAppender(AbstractPrimitiveDescriptor descriptor, ListGeometryAppender appender, PrimitiveInfo info)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.Tag = descriptor;
            geom.PrimitiveType = descriptor.PrimitiveType;
            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable(device, appender.Vertices.ToArray());
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, appender.Indices.ToArray());
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.HasBoundingBox = info.IsBoundingBoxKnown;
            geom.BoundingBox = info.BoundingBox;
            return geom;
        }

        public void Dispose()
        {
            if (this.quad != null)
            {
                this.quad.Dispose();
                this.quad = null;
            }
            if (this.quadlayout != null)
            {
                this.quadlayout.Dispose();
                this.quadlayout = null;
            }
            if (this.cbLuma != null)
            {
                this.cbLuma.Dispose();
                this.cbLuma = null;
            }

            this.VSTri.Dispose();
            this.VSQuad.Dispose();
            this.PSGray.Dispose();
            this.PSLuma.Dispose();
            this.PSPass.Dispose();
            this.PSAlpha.Dispose();

           
        }

    }
}
