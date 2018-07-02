#version 420 core

in Data
{
	flat int instanceID;
	vec3 normal;
} i;

const vec3 materials[] = 
{ 
	vec3(1, 0, 0), vec3(1), vec3(0.3, 1, 0.3), vec3(0.3, 0.3, 1)
	, vec3(0.3, 1, 1), vec3(1, 1, 0.3), vec3(1, 0.3, 1) };

vec3 getMaterial(int id)
{
	return materials[id % 7];
}

out vec4 color;

void main() 
{
	vec3 c = getMaterial(i.instanceID);
	vec3 n = normalize(i.normal);
	vec3 l = normalize(vec3(1, 1, 0));
	color =  vec4(max(dot(n, l), 0.1) * c, 1.0);
	color =  vec4(n, 1.0);
}