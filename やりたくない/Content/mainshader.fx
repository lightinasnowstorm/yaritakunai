#if OPENGL
#define PS_SHADERMODEL ps_3_0
#else
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;


float4 MainPS(float4 coloration: COLOR0, float2 coords: TEXCOORD0) : COLOR
{
	return tex2D(s0, coords)*coloration;
}

float4 PausePS(float4 coloration: COLOR0 ,float2 coords: TEXCOORD0) : COLOR
{
	float4 color = tex2D(s0,coords);
	color *= float4(.5, .5, .5, 1);
	return color*coloration;
}

technique current
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
	pass P1
	{
		PixelShader = compile PS_SHADERMODEL PausePS();
	}
};