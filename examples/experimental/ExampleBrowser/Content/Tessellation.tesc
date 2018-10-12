#version 420 core
layout (vertices = 4) out;
uniform float tesselationLevelInner = 1.0;
uniform float tesselationLevelOuter = 1.0;

out vec4 tcPos[];

void main()
{
	tcPos[gl_InvocationID] = gl_in[gl_InvocationID].gl_Position;
	gl_TessLevelOuter[0] = tesselationLevelOuter;
	gl_TessLevelOuter[1] = tesselationLevelOuter;
	gl_TessLevelOuter[2] = tesselationLevelOuter;
	gl_TessLevelOuter[3] = tesselationLevelOuter;
	gl_TessLevelInner[0] = tesselationLevelInner;
	gl_TessLevelInner[1] = tesselationLevelInner;
}