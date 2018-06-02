#version 430 core

uniform mat4 camera;
uniform float lightRange = 1;

in vec3 position;
in vec4 lightData;

out Data
{
	vec2 uv;
	vec3 lightPosition;
	vec3 lightColor;
} dataOut;

void main() 
{
	const vec3 materials[] = 
	{ 
		vec3(1), vec3(1), vec3(0, 1, 0), vec3(0, 0, 1)
		, vec3(0, 1, 1), vec3(1, 1, 0), vec3(1, 0, 1) };

	//vec4 pos = camera * vec4(position + lightData.xyz, 1.0);// * 0.9 + center;
	vec4 pos = vec4(position.xz, 0, 1.0);
	dataOut.uv = pos.xy / pos.w * 0.5 + 0.5;
	dataOut.lightPosition = lightData.xyz;
	dataOut.lightColor = materials[uint(lightData.w) % 7];
	gl_Position = pos;
}