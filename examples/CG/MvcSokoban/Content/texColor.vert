#version 430 core

uniform mat4 camera;

in vec2 position;
in vec2 instanceTranslate;
in float texId;

out vec2 uv;
out float tileType;

void main() 
{
	uv = position;
	tileType = texId;
	gl_Position = camera * vec4(position + instanceTranslate, 0, 1);
}