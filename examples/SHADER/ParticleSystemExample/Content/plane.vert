#version 430 core				

uniform mat4 camera;

in vec4 position;

void main() 
{
	gl_Position = camera * position;
}