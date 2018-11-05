#version 430 core

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec2 uv;

out Data
{
	vec3 position;
	vec3 normal;
	flat uint material;
} outData;

void main() 
{
	outData.position = position;
	outData.normal = normal;
	outData.material = uint(uv.x);
	
	gl_Position = camera * vec4(position, 1.0);
}