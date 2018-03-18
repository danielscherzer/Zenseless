#version 430 core

in vec3 pos;
in vec3 n;

out vec4 color;

void main() 
{
	vec3 normal = normalize(n);

	color =  vec4(abs(normal), 1);
}