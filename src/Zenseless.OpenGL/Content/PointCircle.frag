#version 430 core

uniform vec4 color = vec4(1);

void main() 
{
	float f = distance(gl_PointCoord, vec2(0.5));
	gl_FragColor.rgb = color.rgb;
	gl_FragColor.a = 1 - smoothstep(0.4, 0.5, f);
}