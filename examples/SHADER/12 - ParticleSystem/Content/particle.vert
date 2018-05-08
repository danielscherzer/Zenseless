#version 430 core				

uniform mat4 camera;
uniform float pointSize = 1;

in vec4 position;
in float fade;

out float fadeFrag;

void main() 
{
	fadeFrag = fade;
	vec4 pos = camera * position;
	gl_PointSize = (1 - pos.z / pos.w) * 1000 * pointSize;
	gl_Position = pos;
}