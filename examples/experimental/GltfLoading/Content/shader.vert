#version 430 core
uniform mat4 camera;

in vec4 position;
in vec3 normal;

out Data
{
	vec3 normal;
	vec3 position;
} o;

void main() 
{
	o.normal = normal;
	o.position = position.xyz;
	gl_Position = camera * position;
}