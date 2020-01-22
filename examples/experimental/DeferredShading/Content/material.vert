#version 430 core

uniform mat4 camera;

in vec4 instanceData;
in vec3 position;
in vec3 normal;
in vec2 uv;

out Data
{
	vec3 position;
	vec3 normal;
	flat float material; //flat important for NVIDIA - INTEL does not care
} outData;

void main() 
{
	vec3 pos = instanceData.xyz + position;
	outData.position = pos;
	outData.normal = normal; // would need to rotate if there is model to world rotation
	outData.material = instanceData.w; // material id
	
	gl_Position = camera * vec4(pos, 1.0);
}