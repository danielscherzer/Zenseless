#version 430 core
uniform vec3 ambient;

in blockData
{
	vec3 normal;
	vec2 uv;
} i;

out vec4 color;

void main() 
{
	color = vec4(i.normal, 1);
}