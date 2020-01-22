#version 430 core	

uniform mat4 camera;
uniform mat4 light;

in vec4 position;
in vec3 normal;

out blockData
{
	vec4 position_LS;
	vec3 position;
	vec3 normal;
} o;

void main() 
{
	o.position = position.xyz;
	o.position_LS = light * position;
	o.normal = normal;
	gl_Position = camera * position;
}