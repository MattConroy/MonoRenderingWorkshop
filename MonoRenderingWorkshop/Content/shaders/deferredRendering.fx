#include "common.fxh"

matrix World;
matrix View;
matrix Projection;
matrix InverseViewProjection;

float3 LightAttenuation;
float4 LightDirection;
float3 LightPosition;
float3 LightColour;

DECLARE_TEXTURE(PotatoBuffer, 0);
DECLARE_TEXTURE(PositionBuffer, 2);
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
	output.Position = input.Depth.x / input.Depth.y;
	output.Normal = 0.5 * normalize(input.Normal) + 0.5;
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
    float4 normalData = SAMPLE_TEXTURE(NormalBuffer, input.TexCoord);
    //tranform normal back into [-1,1] range
    float3 normal = normalize(2.0 * normalData.xyz - 1.0);
	//read depth
	float positionData = SAMPLE_TEXTURE(PositionBuffer, input.TexCoord).r;
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
		
	// output.Colour = positionData;
	// output.Colour = normalData;
	// output.Colour = normalData;
	// output.Colour = float4(input.TexCoord.xy, 1, 1);
	// output.Colour = float4(1,1,1,1);
	// output.Colour.a = 1.0;
	return output;
}

TECHNIQUE(ClearBuffer, ClearBufferVertexMain, ClearBufferPixelMain)
TECHNIQUE(RenderGeometry, GeometryVertexMain, GeometryPixelMain)
TECHNIQUE(RenderLighting, LightingVertexMain, LightingPixelMain)