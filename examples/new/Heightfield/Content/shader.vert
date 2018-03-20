#version 430 core

uniform mat4 camera;
uniform sampler2D texHeightfield;

in vec4 position;
in vec3 normal;
in vec2 uv;

out vec3 n;
out vec2 uvs;

void main() 
{
	n = normal;
	uvs = uv;
	float height = texture(texHeightfield, uv).r;
	vec4 newPos = position;
	newPos.y = height * 1f;
	gl_Position = camera * newPos;
}