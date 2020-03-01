#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass { VertexShader = compile VS_SHADERMODEL vsname (); PixelShader = compile PS_SHADERMODEL psname(); } }

#define DECLARE_TEXTURE(Name, index) \
    Texture2D<float4> Name : register(t##index); \
    sampler Name##Sampler : register(s##index)

#define SAMPLE_TEXTURE(Name, texCoord)  Name.Sample(Name##Sampler, texCoord)


float3 ComputeLightContribution(float3 worldPosition, float3 worldNormal,
	float3 lightPosition, float4 lightDirection, float3 lightAttenuation, float3 lightColour)
{
//   worldNormal = normalize(worldNormal);
// 	 lightDirection.xyz = normalize(float3(-1,-1,-1));
// 	 lightPosition = float3(0,0,0);
// 	lightColour = float3(1,1,1);
// 	lightAttenuation = float3(1,0,0);

	float3 directionToLight = worldPosition - lightPosition;
	float distance = length(directionToLight);
	directionToLight = normalize(directionToLight);

	float3 lightNormal = lerp(directionToLight, lightDirection.xyz, lightDirection.w);
	float directionalIntensity = dot(worldNormal, -lightNormal);

	float3 diffuse = lightColour * directionalIntensity;
	float attenuation = lightAttenuation.x +
						lightAttenuation.y * distance +
						lightAttenuation.z * distance * distance;
	return saturate(diffuse / attenuation);
}