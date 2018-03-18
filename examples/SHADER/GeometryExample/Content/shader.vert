#version 430 core				

uniform float time;

in vec4 in_position;
in vec2 in_velocity;

void main() 
{
	gl_PointSize = 100.0;
	vec2 newPos = in_position.xy + time * in_velocity;
	gl_Position = vec4(newPos, 0.0, 1.0);
}