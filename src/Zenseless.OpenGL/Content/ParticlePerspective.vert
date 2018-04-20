#version 130 //gl_DepthRange

uniform mat4 mvp = mat4(1.0);
uniform float pointSize = 1.0;

in vec4 position;

void main() 
{
	vec4 pos = mvp * position;
	gl_PointSize = (1.0 - pos.z / pos.w) * pointSize;
	gl_Position = pos;
}