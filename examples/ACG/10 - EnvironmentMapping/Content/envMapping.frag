#version 430 core
#include "projection.glsl"
//!#define SOLUTION

uniform sampler2D envMap;

uniform vec3 cameraPosition;

in Data
{
	vec3 position;
	vec3 normal;
} inData;

out vec4 color;

#ifdef SOLUTION
uniform float reflective = 0;
#endif

void main() 
{
	color = vec4(1);
#ifdef SOLUTION
	vec3 dir = normalize(inData.position - cameraPosition);
	vec3 normal = normalize(inData.normal);

	vec3 r = reflect(dir, normal);
	vec4 reflectedColor = texture(envMap, projectLongLat(r));

	vec3 t = refract(dir, normal, 0.95);
	vec4 refractedColor = texture(envMap, projectLongLat(t));

	color = mix(refractedColor, reflectedColor, reflective);
#endif
}