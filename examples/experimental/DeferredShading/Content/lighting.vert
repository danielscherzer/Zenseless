#version 430 core
#include "helper.glsl"

uniform mat4 camera;
uniform float lightRange = 1;

in vec3 position;
in vec4 lightData;

out Data
{
	vec3 lightPosition;
	vec3 lightColor;
} dataOut;

void main() 
{
	vec3 lightPosition = lightData.xyz;
	vec4 pos = camera * vec4(position * lightRange + lightPosition, 1.0);

	dataOut.lightPosition = lightPosition;
	dataOut.lightColor = getMaterial(lightData.w);
	gl_Position = pos;
}