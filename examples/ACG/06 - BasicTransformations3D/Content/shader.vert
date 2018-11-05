#version 430 compatibility

uniform mat4 camera = mat4(1.0);

in vec4 position;
in vec3 normal;
in mat4 instanceTransform;

out vec3 n;

void main() 
{
	n = normal;

	gl_Position = camera * instanceTransform * position;
}