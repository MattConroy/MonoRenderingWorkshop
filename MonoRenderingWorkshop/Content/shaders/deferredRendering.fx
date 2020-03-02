﻿#include "common.fxh"

matrix World;
matrix View;
matrix Projection;
matrix InverseViewProjection;

float3 LightAmbientColour;
float3 LightDiffuseColour;
float3 LightPosition;
float4 LightDirection;
float3 LightAttenuation;

DECLARE_TEXTURE(PositionBuffer, 0);
DECLARE_TEXTURE(NormalBuffer, 1);

struct ClearBufferVertexInput { float4 Position : POSITION0; };
struct ClearBufferVertexOutput { float4 Position : SV_POSITION; };
struct ClearBufferPixelOutput
{
    float4 Position: COLOR0;
    float4 Normal : COLOR1;
};

ClearBufferVertexOutput ClearBufferVertexMain(in ClearBufferVertexInput input)
{
	ClearBufferVertexOutput output = (ClearBufferVertexOutput)0;
    output.Position = mul(input.Position, Projection);
	return output;
}
ClearBufferPixelOutput ClearBufferPixelMain(ClearBufferVertexOutput input)
{
	ClearBufferPixelOutput output = (ClearBufferPixelOutput)0;
	output.Position = float4(1.0, 1.0, 1.0, 1.0);
    output.Normal = float4(0.5, 0.5, 0.5, 0.5);
    return output;
}

struct GeometryVertexInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};
struct GeometryVertexOutput
{
	float4 Position : SV_POSITION;
	float4 Normal : NORMAL0;
	float3 WorldPos : TEXCOORD0;
};
struct GeometryPixelOutput
{
    half4 Position: COLOR0;
    half4 Normal : COLOR1;
};

GeometryVertexOutput GeometryVertexMain(in GeometryVertexInput input)
{
	GeometryVertexOutput output;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, World);
	output.WorldPos = worldPosition.xyz;
	return output;
}
GeometryPixelOutput GeometryPixelMain(GeometryVertexOutput input) 
{
	GeometryPixelOutput output;
	output.Position = float4(1.0, 1.0, 1.0, 1.0);
    output.Normal = float4(0.5, 0.5, 0.5, 0.5);
	return output;
}

struct LightingVertexInput
{
	float3 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};
struct LightingVertexOutput
{
	float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};
struct LightingPixelOutput
{
	float4 Colour : COLOR;
};

LightingVertexOutput LightingVertexMain(LightingVertexInput input)
{
    LightingVertexOutput output;
    output.Position = float4(input.Position, 1.0);
    output.TexCoord = input.TexCoord;
    return output;
}
LightingPixelOutput LightingPixelMain(LightingVertexOutput input)
{
    LightingPixelOutput output;

	float4 position = float4(0.0, 0.0, 0.0, 1.0);
	float4 normal = float4(0.0, 1.0, 0.0, 0.0);

	output.Colour = float4(ComputeLightContribution(position.xyz, normal.xyz, LightPosition,
		LightDirection, LightAttenuation, LightDiffuseColour, LightAmbientColour), 1.0);

	output.Colour = float4(0.0, 0.0, 0.0, 1.0);
		
	return output;
}

TECHNIQUE(ClearBufferPass, ClearBufferVertexMain, ClearBufferPixelMain)
TECHNIQUE(GeometryPass, GeometryVertexMain, GeometryPixelMain)
TECHNIQUE(LightingPass, LightingVertexMain, LightingPixelMain)