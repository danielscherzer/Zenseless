#version 430 core
uniform sampler2D texShadowMap;

uniform vec3 ambient;

in blockData
{
	vec4 position_LS;
	vec3 normal;
	vec2 uv;
} i;

out vec4 color;

void main() 
{
	vec3 coord = i.position_LS.xyz / i.position_LS.w;
	float depth = texture(texShadowMap, coord.xy * .5 + 0.5).r;
	color = depth + 0.001 > coord.z ? vec4(1) : vec4(0);
}