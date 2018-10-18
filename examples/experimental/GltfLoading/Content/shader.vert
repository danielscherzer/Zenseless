#version 430 core
uniform mat4 camera;
uniform mat4 world;

in vec4 position;
in vec3 normal;

out Data
{
	vec3 normal;
	vec3 position;
} o;

void main() 
{
	o.normal = mat3(world) * normal;
	vec4 position_w = world * position;
	o.position = position_w.xyz;
	gl_Position = camera * position_w;
}