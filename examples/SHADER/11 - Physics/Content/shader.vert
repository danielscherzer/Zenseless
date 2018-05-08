#version 430 core

uniform float time;
uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec3 instancePosition;
in float instanceScale;

out vec3 n;

void main() 
{
	n = normal;

	vec3 pos = instanceScale * position + instancePosition;
	gl_Position = camera * vec4(pos, 1.0);
}