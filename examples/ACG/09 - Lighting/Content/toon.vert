#version 430 core				

uniform mat4 camera;

in vec3 position;
in vec3 normal;

out vec3 pos;
out vec3 n;


void main() 
{
	pos = position;
	n = normal;

	gl_Position = camera * vec4(position, 1.0);
}