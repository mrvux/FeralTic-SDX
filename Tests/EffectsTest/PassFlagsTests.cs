using FeralTic.DX11;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Tests.EffectsTest
{
    [TestClass]
    public class EffectPassTests : RenderDeviceTestBase
    {
        private EffectInstance shader;

         [TestInitialize()]
        public override void Initialize()
        {
            base.Initialize();

            var effect = DX11Effect.CompileFromString(@"
struct VS_IN
{
	float4 PosO : POSITION;
	float2 TexCd : TEXCOORD0;
};

struct vs2ps
{
    float4 PosWVP: SV_POSITION;
    float2 TexCd: TEXCOORD0;
};

cbuffer cbTest : register(b0)
{
    float4 offset;
};

Texture2D tex;

SamplerState linSamp
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};


vs2ps VS(VS_IN input)
{
    vs2ps Out = (vs2ps)0;
    Out.PosWVP  =input.PosO + offset;
    Out.TexCd = input.TexCd + tex.Load(int3(0,0,0));
    return Out;
}

technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
	}
}");
            this.shader = new EffectInstance(this.Device, effect);
        }

        [TestMethod()]
        public void BindAllTest()
        {
            this.RenderContext.Context.VertexShader.Set(null);
            this.RenderContext.Context.VertexShader.SetShaderResource(0, null);

            var tex = this.Device.DefaultTextures.WhiteTexture;
            this.shader.SetByName("tex", tex.ShaderView);
            var pass = this.shader.GetPass(0);
            pass.Apply(this.RenderContext);

            Assert.IsNotNull(this.RenderContext.Context.VertexShader.Get());
            Assert.IsNotNull(this.RenderContext.Context.VertexShader.GetShaderResources(0, 1)[0]);
        }

        [TestMethod()]
        public void IgnoreShaderFlag()
        {
            this.RenderContext.Context.VertexShader.Set(null);
            this.RenderContext.Context.VertexShader.SetShaderResource(0, null);

            var tex = this.Device.DefaultTextures.WhiteTexture;
            this.shader.SetByName("tex", tex.ShaderView);
            var pass = this.shader.GetPass(0);
            pass.Apply(this.RenderContext, 2);

            Assert.IsNull(this.RenderContext.Context.VertexShader.Get());
            Assert.IsNotNull(this.RenderContext.Context.VertexShader.GetShaderResources(0, 1)[0]);
        }

        [TestMethod()]
        public void IgnoreResourceFlagFlag()
        {
            this.RenderContext.Context.VertexShader.Set(null);
            this.RenderContext.Context.VertexShader.SetShaderResource(0, null);

            var tex = this.Device.DefaultTextures.WhiteTexture;
            this.shader.SetByName("tex", tex.ShaderView);
            var pass = this.shader.GetPass(0);
            pass.Apply(this.RenderContext, EffectPassFlags.IgnoreResourceBlocks);

            Assert.IsNotNull (this.RenderContext.Context.VertexShader.Get());
            Assert.IsNull(this.RenderContext.Context.VertexShader.GetShaderResources(0, 1)[0]);
        }
    }
}
