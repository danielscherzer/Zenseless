#version 430 core				

uniform mat4 camera;

in vec4 position;
in vec3 normal;
in mat4 instanceTransform;

out vec3 n;

void main() 
{
	n = normal;

	gl_Position = camera * instanceTransform * position;
}