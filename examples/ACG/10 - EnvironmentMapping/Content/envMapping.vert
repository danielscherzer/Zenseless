#version 430 core

uniform mat4 camera;

in vec3 position;
in vec3 normal;

out Data
{
	vec3 position;
	vec3 normal;
} outData;


void main() 
{
	outData.position = position;
	outData.normal = normal;

	gl_Position = camera * vec4(position, 1.0);
}