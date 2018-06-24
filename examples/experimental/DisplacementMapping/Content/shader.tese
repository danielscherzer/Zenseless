#version 420 core
#include "noise2D.glsl"

uniform mat4 camera;
uniform sampler2D texDisplacement;
uniform int instanceSqrt = 5;

layout (quads, fractional_odd_spacing, ccw) in;

out Data
{
	flat int instanceID;
} o;

in vec4 tcPos[gl_MaxPatchVertices];
in vec2 tcTexCoord[gl_MaxPatchVertices];

patch in int instanceID;

vec2 interpolate(vec2 v1, vec2 v2, vec2 v3, vec2 v4)
{
	vec2 aX = mix(v1, v2, gl_TessCoord.x);
	vec2 bX = mix(v4, v3, gl_TessCoord.x);
	return mix(aX, bX, gl_TessCoord.y);
}

vec4 interpolate(vec4 v1, vec4 v2, vec4 v3, vec4 v4)
{
	vec4 aX = mix(v1, v2, gl_TessCoord.x);
	vec4 bX = mix(v4, v3, gl_TessCoord.x);
	return mix(aX, bX, gl_TessCoord.y);
}

float displacement(vec2 coord)
{
	float d = snoise(coord * 5);
	d += snoise(coord * 20) * 0.5;
	d += snoise(coord * 40) * 0.25;
	d += snoise(coord * 500) * 0.01;
	d = max(d, -0.1);
	return d * 1.5;
}

void main() 
{
	vec4 pos = interpolate(tcPos[0], tcPos[1], tcPos[2], tcPos[3]);
	vec2 texCoord = interpolate(tcTexCoord[0], tcTexCoord[1], tcTexCoord[2], tcTexCoord[3]);
	pos.y = displacement(texCoord);

	gl_Position = camera * pos;
	o.instanceID = instanceID;
}