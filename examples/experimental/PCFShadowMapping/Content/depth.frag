#version 430 core

in blockData
{
	vec4 position;
} i;

out vec4 color;

void main() 
{
	color = vec4(1);
	float delta = 0.001;
	color = vec4(delta + i.position.z / i.position.w);
}