#version 430 core

uniform float time;
uniform float pointSize;

in vec4 position;
in vec2 velocity;

void main() 
{
	gl_PointSize = pointSize;
	vec2 newPos = position.xy + time * velocity;
	gl_Position = vec4(newPos, 0.0, 1.0);
}