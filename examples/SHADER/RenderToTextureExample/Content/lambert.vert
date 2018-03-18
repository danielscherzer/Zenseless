#version 430 core

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec2 uv;

out vec3 v_pos;
out vec3 v_n;
out vec2 v_uv;

void main() 
{
	v_pos = position;
	v_n = normal;
	v_uv = uv;
	
	gl_Position = camera * vec4(position, 1.0);
}