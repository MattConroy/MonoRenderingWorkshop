#include "common.fxh"

matrix World;
matrix View;
matrix Projection;

float3 LightAmbientColour;
float3 LightDiffuseColour;
float3 LightPosition;
float4 LightDirection;
float3 LightAttenuation;

struct VertexInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};
struct VertexOutput
{
	float4 Position : SV_POSITION;
	float4 Normal : NORMAL0;
	float3 WorldPos : TEXCOORD0;
};
struct PixelOutput
{
	float4 Colour : COLOR;
};

VertexOutput VertexMain(in VertexInput input)
{
	VertexOutput output;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, World);
	output.WorldPos = worldPosition.xyz;
	return output;
}
PixelOutput AmbientPixelMain(VertexOutput input) 
{
	PixelOutput output;
	output.Colour = float4(0.0, 0.0, 0.0, 1.0);
	return output;
}
PixelOutput DiffusePixelMain(VertexOutput input) 
{
	PixelOutput output;
	output.Colour = float4(ComputeLightContribution(input.WorldPos, normalize(input.Normal.xyz),
		LightPosition, LightDirection, LightAttenuation, LightDiffuseColour, LightAmbientColour), 1.0);
	return output;
}

TECHNIQUE(DepthPrePass, VertexMain, AmbientPixelMain)
TECHNIQUE(LightingPass, VertexMain, DiffusePixelMain)