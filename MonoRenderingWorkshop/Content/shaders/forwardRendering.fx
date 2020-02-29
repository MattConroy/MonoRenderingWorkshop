#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix World;
matrix View;
matrix Projection;
matrix WorldInverseTranspose;

static const int MAX_LIGHT_COUNT = 8;
float3 LightDirection[MAX_LIGHT_COUNT];
float3 LightColour[MAX_LIGHT_COUNT];

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Normal : NORMAL0;
	float4 Color : COLOR0;
};

VertexShaderOutput VertexMain(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
	
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, World);
	return output;
}

float4 PixelMain(VertexShaderOutput input) : COLOR
{
	float3 lightColour = float3(0,0,0);
	[unroll(MAX_LIGHT_COUNT)]
	for(int i = 0 ; i < MAX_LIGHT_COUNT ; i++)
	{
		float lightIntensity = dot(input.Normal.xyz, LightDirection[i]);
		lightColour += saturate(LightColour[i] * lightIntensity);
	}
    // float lightIntensity = dot(input.Normal.xyz, LightDirection);
    // float3 lightColour = saturate(LightColour * LightIntensity * lightIntensity);
	 return float4(lightColour.rgb, 1) + AmbientColor * AmbientIntensity;
	// return AmbientColor * AmbientIntensity;
}

technique Forward
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL VertexMain();
		PixelShader = compile PS_SHADERMODEL PixelMain();
	}
};