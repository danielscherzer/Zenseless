#version 430 core

uniform Uniforms
{
	mat4 camera;
};

in vec3 position;
in vec3 normal;
in vec2 uv;

out vec3 n;
out vec2 uvs;

void main() 
{
	n = normal;
	uvs = uv;
	gl_Position = camera * vec4(position, 1.0);
}