#version 430 core
uniform sampler2D texNormalMaterial;
uniform sampler2D texPosition;
uniform float lightIntensity = 1;
uniform float lightRange = 1;

in Data
{
	vec2 uv;
	vec3 lightPosition;
	vec3 lightColor;
} dataIn;

out vec4 color;

void main() 
{
	const vec3 materials[] = 
	{ 
		vec3(1), vec3(1), vec3(0, 1, 0), vec3(0, 0, 1)
		, vec3(0, 1, 1), vec3(1, 1, 0), vec3(1, 0, 1) };

	vec3 position = texture(texPosition, dataIn.uv).xyz;
	vec3 lightVec = dataIn.lightPosition - position;
	float dist = length(lightVec);
	float attenuation = clamp(1.0 - dist / lightRange, 0.0, 1.0);

	vec3 light = normalize(lightVec);
	vec4 inMaterial = texture(texNormalMaterial, dataIn.uv);
	vec3 normal = inMaterial.xyz;
	vec3 albedo = materials[uint(inMaterial.w) % 7];

	vec3 diffuse = max(0, dot(normal, light)) * albedo * dataIn.lightColor * lightIntensity * attenuation;

	color =  vec4(diffuse, 1);
//	color =  vec4(1);
}