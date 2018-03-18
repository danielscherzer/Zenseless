#version 430 core				

uniform float time;
uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec3 instancePosition;

out vec3 n;

void main() 
{
	n = normal;

	vec3 pos = position + instancePosition;
	gl_Position = camera * vec4(pos, 1.0);
}