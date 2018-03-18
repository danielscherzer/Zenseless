#version 430 core

in blockData
{
	vec4 position;
} i;

out vec4 color;

void main() 
{
	color = vec4(i.position.z / i.position.w);
}