#version 430 core
in vec4 position;
in vec3 normal;

out Data
{
	vec3 normal;
} o;

void main() 
{
	o.normal = normal;
	vec4 pos = position;
	pos.xyz *= 0.001;
	gl_Position = pos;
}