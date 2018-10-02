//?#version 120

const float PI = 3.14159265359;

vec2 projectLongLat(vec3 direction) {
	float theta = atan(direction.x, -direction.z) + PI;
	float phi = acos(-direction.y);
	return vec2(theta / (2*PI), phi / PI);
}
