#version 430 core

uniform sampler2D envMap;

//uniform vec3 cameraPosition;

in vec3 pos;
in vec3 n;

out vec4 color;
const float PI = 3.14159265359;

vec2 projectLongLat(vec3 direction) {
  float theta = atan(direction.x, -direction.z) + PI;
  float phi = acos(-direction.y);
  return vec2(theta / (2*PI), phi / PI);
}

void main() 
{
	//vec3 normal = normalize(n);
	vec3 dir = normalize(pos); //for sky dome camera should stay fixed in the center
	color = texture(envMap, projectLongLat(dir));
}