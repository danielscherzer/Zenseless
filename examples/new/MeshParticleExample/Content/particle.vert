#version 430 core

uniform mat4x4 camera;

in vec3 instancePosition;
in vec3 instanceColor;
in float instanceRotation;

in vec3 position;
in vec3 normal;

out vec3 varNormal;
out vec3 baseColor;

vec3 rotate(vec3 pos, float angle)
{
	float c = cos(angle);
	float s = sin(angle);
	mat3 rot = mat3(vec3(c, 0, s), vec3(0, 1, 0), vec3(-s, 0, c));
	return rot * pos;
}

void main() 
{
	vec3 pos = rotate(position, instanceRotation) + instancePosition;
	baseColor = instanceColor;
	varNormal = normal;
	gl_Position = camera * vec4(pos, 1.0);
}