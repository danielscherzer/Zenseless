#version 430 core

out vec4 color;

void main() 
{
	const vec4 color1 = vec4(0.6, 0.0, 0.0, 1.0);
	const vec4 color2 = vec4(0.9, 0.7, 1.0, 0.0);

	float f = distance(gl_PointCoord, vec2(0.5));
	color = mix(color1, color2, smoothstep(0.41, 0.5, f));
}