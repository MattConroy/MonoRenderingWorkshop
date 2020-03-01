#include "common.fxh"

matrix World;
matrix View;
matrix Projection;

static const int MAX_LIGHT_COUNT = 4;
float3 LightAttenuation[MAX_LIGHT_COUNT];
float4 LightDirection[MAX_LIGHT_COUNT];
float3 LightPosition[MAX_LIGHT_COUNT];
float3 LightColour[MAX_LIGHT_COUNT];

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
PixelOutput PixelMain(VertexOutput input) 
{
	PixelOutput output;
	float3 lightColour = float3(0,0,0);
	[unroll(MAX_LIGHT_COUNT)]
	for(int i = 0 ; i < MAX_LIGHT_COUNT ; i++)
	{
		lightColour += ComputeLightContribution(input.WorldPos, normalize(input.Normal.xyz),
			LightPosition[i], LightDirection[i], LightAttenuation[i], LightColour[i]);
	}
	output.Colour = float4(lightColour.rgb, 1);
	return output;
}

TECHNIQUE(Forward, VertexMain, PixelMain)