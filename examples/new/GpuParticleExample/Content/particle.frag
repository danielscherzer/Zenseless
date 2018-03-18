#version 430 core
in vec3 baseColor;

out vec4 color;

void main() 
{
	float dist = distance(gl_PointCoord, vec2(0.5));
	if(dist > 0.4) discard;
	color = vec4(baseColor, 1);
}