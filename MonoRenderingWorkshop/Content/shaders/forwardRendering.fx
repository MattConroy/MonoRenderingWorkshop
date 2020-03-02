﻿#include "common.fxh"

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
PixelOutput PixelMain(VertexOutput input) 
{
	PixelOutput output;
	float directionalIntensity = dot(normalize(input.Normal.xyz), -LightDirection.xyz);

	float3 diffuse = LightDiffuseColour * directionalIntensity;
	float3 colour = saturate(diffuse + LightAmbientColour);
	output.Colour = float4(colour, 1.0);
	return output;
}

TECHNIQUE(Forward, VertexMain, PixelMain)