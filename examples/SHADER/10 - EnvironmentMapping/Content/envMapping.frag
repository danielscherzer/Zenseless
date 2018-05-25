#version 430 core

uniform sampler2D envMap;

uniform vec3 cameraPosition;

in Data
{
	vec3 position;
	vec3 normal;
} inData;

out vec4 color;

#ifdef SOLUTION
uniform float reflective = 0f;

vec2 projectLongLat(vec3 direction)
{
	const float PI = 3.14159265359;
	float theta = atan(direction.x, -direction.z) + PI;
	float phi = acos(-direction.y);
	return vec2(theta / (2*PI), phi / PI);
}
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