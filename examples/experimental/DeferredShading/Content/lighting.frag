#version 430 core
#include "helper.glsl"

uniform sampler2D texNormalMaterial;
uniform sampler2D texPosition;
uniform float lightIntensity = 1;
uniform float lightRange = 1;

in Data
{
	vec3 lightPosition;
	vec3 lightColor;
} dataIn;

out vec4 color;

void main() 
{
	ivec2 uv = ivec2(gl_FragCoord);
	vec3 position = texelFetch(texPosition, uv, 0).xyz;
	vec3 toLight = dataIn.lightPosition - position;
	float dist = length(toLight);
	float attenuation = clamp(1.0 - dist / lightRange, 0.0, 1.0);
	attenuation *= attenuation * 4.0 * 3.1415;
	
	toLight = normalize(toLight);
	vec4 inNormalMaterial = texelFetch(texNormalMaterial, uv, 0);
	vec3 normal = inNormalMaterial.xyz;
	vec3 albedo = getMaterial(inNormalMaterial.w);

	float lambert = max(0, dot(normal, toLight));
	vec3 light = dataIn.lightColor * lightIntensity * attenuation;
	vec3 diffuse = lambert * albedo * light;

	color =  vec4(diffuse, 1);
}