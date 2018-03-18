#version 430 core
in vec3 baseColor;

out vec4 color;

void main() 
{
	const vec4 color2 = vec4(0.0);

	float f = distance(gl_PointCoord, vec2(0.5));
	float weight = smoothstep(0.31, 0.5, f);
	color = mix(vec4(baseColor, 1), color2, weight);
}