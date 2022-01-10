#version 430 core

uniform vec4 color = vec4(1);

out vec4 fragColor;
void main() 
{
	float f = distance(gl_PointCoord, vec2(0.5));
	fragColor = vec4(color.rgb, 1.0 - smoothstep(0.4, 0.5, f));
}