#version 430 core

in vec2 uv;

out vec4 color;

void main() 
{
	vec2 uv10 = floor(uv * 10.0f);
	bool black = 1.0 > mod(uv10.x + uv10.y, 2.0f);
	color = black ? vec4(0, 0, 0, 1) : vec4(1, 1, 0, 1);
}