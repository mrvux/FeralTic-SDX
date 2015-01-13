Texture2D tex : TEXTURE; 

cbuffer cbLuma : register(b0)
{
	float luma;
};

SamplerState linSamp
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};


struct vs2ps
{
    float4 position: SV_POSITION;
    float2 uv : TEXCOORD0;
};


vs2ps VSFullTri( uint vertexID : SV_VertexID )
{
    vs2ps result;
    result.uv = float2((vertexID << 1) & 2, vertexID & 2);
    result.position = float4(result.uv * float2(2.0f, -2.0f) + float2(-1.0f, 1.0f), 0.0f, 1.0f);
	return result;
}

float4 PS(vs2ps input): SV_Target
{
    return tex.Sample( linSamp, input.uv);
}

float4 PSGray(vs2ps input) : SV_Target
{
	return tex.Sample(linSamp, input.uv).rrrr;
}

float4 PSLuma(vs2ps input) : SV_Target
{
	return tex.Sample(linSamp, input.uv) * luma;
}

technique10 FullScreenTriangleVSOnly
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VSFullTri() ) );
	}
}


technique10 FullScreenTriangle
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VSFullTri() ) );
		SetPixelShader( CompileShader(ps_4_0, PS()));
	}
}

technique10 FullScreenTriangleGray
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, VSFullTri()));
		SetPixelShader(CompileShader(ps_4_0, PSGray()));
	}
}

technique10 FullScreenTriangleLuma
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, VSFullTri()));
		SetPixelShader(CompileShader(ps_4_0, PSLuma()));
	}
}