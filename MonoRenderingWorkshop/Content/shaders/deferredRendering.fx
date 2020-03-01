#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
#include "common.fx"

matrix World;
matrix View;
matrix Projection;
matrix InverseViewProjection;

float3 LightAttenuation;
float4 LightDirection;
float3 LightPosition;
float3 LightColour;

texture PositionBuffer;
texture NormalBuffer;
sampler PositionSampler = sampler_state
{
    Texture = (PositionBuffer);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = POINT;
    MinFilter = POINT;
    Mipfilter = POINT;
};
sampler NormalSampler = sampler_state
{
    Texture = (NormalBuffer);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = POINT;
    MinFilter = POINT;
    Mipfilter = POINT;
};

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
	output.Position = float4(1,1,1,1);
    output.Normal = float4(0.5,0.5,0.5,0.5);
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
	float2 Depth : TEXCOORD0;
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
	output.Depth = output.Position.zw;
	return output;
}
GeometryPixelOutput GeometryPixelMain(GeometryVertexOutput input) 
{
	GeometryPixelOutput output;
	output.Position = (input.Depth.x / 50);
	output.Normal = (1 + normalize(input.Normal)) * 0.5;
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
    output.Position = float4(input.Position, 1);
    output.TexCoord = input.TexCoord;// - halfPixel;
    return output;
}

LightingPixelOutput LightingPixelMain(LightingVertexOutput input)
{
    LightingPixelOutput output;
    //get normal data from the normalMap
    float4 normalData = tex2D(NormalSampler, input.TexCoord);
    //tranform normal back into [-1,1] range
    float3 normal = 2.0f * normalData.xyz - 1.0f;
	//read depth
	float positionData = tex2D(PositionSampler, input.TexCoord).r;
	//compute screen-space position
	float4 position;
	position.x = input.TexCoord.x * 2.0f - 1.0f;
	position.y = -(input.TexCoord.y * 2.0f - 1.0f);
	position.z = positionData;
	position.w = 1.0f;
	//transform to world space
	position = mul(position, InverseViewProjection);
	position /= position.w;

	output.Colour = float4(ComputeLightContribution(position.xyz, normal.xyz,
		LightPosition, LightDirection, LightAttenuation, LightColour), 1);
		
	output.Colour = normalData;
	output.Colour = float4(input.TexCoord.xy, 1, 1);
	// output.Colour = float4(1,1,1,1);
	
	return output;
}

technique ClearBuffer
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL ClearBufferVertexMain();
		PixelShader = compile PS_SHADERMODEL ClearBufferPixelMain();
	}
};

technique RenderGeometry
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL GeometryVertexMain();
		PixelShader = compile PS_SHADERMODEL GeometryPixelMain();
	}
};

technique RenderLighting
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL LightingVertexMain();
		PixelShader = compile PS_SHADERMODEL LightingPixelMain();
	}
};