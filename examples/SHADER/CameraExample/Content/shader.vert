#version 430 core				

uniform float time;
uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec3 instancePosition;
in vec3 instanceSpeed;

out vec3 n;

void main() 
{
	n = normal;

	vec3 pos = position;
	pos += instancePosition + time * instanceSpeed;

	gl_Position = camera * vec4(pos, 1.0);
}