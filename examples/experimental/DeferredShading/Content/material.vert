#version 430 core

uniform mat4 camera;

in vec4 lightData;
in vec3 position;
in vec3 normal;
in vec2 uv;

out Data
{
	vec3 position;
	vec3 normal;
	flat float material; //important for NVIDIA - INTEL does not care
} outData;

void main() 
{
	vec3 pos = lightData.xyz + position;
	outData.position = pos;
	outData.normal = normal;
	outData.material = lightData.w;
	
	gl_Position = camera * vec4(pos, 1.0);
}