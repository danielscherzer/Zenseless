#version 430 core
#include "projection.glsl"

uniform sampler2D envMap;

in vec3 pos;
in vec3 n;

out vec4 color;

void main() 
{
	vec3 dir = normalize(pos); //for sky dome camera should stay fixed in the center
	color = texture(envMap, projectLongLat(dir));
}