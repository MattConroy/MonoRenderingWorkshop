
float3 ComputeLightContribution(float3 worldPosition, float3 worldNormal,
	float3 lightPosition, float4 lightDirection, float3 lightAttenuation, float3 lightColour)
{
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