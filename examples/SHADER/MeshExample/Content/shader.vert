#version 430 core				
in vec3 position;
in vec3 normal;

out vec3 n;

void main() 
{
	n = normal;
	gl_Position = vec4(position, 1.0);
}