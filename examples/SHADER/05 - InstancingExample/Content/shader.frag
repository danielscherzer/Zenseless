#version 430 core

in vec3 var_color;

out vec4 color;

void main() 
{
	color = vec4(var_color, 1.0);
}