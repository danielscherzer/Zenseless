const vec3 materials[] = 
{ 
	vec3(1), vec3(1), vec3(0.3, 1, 0.3), vec3(0.3, 0.3, 1)
	, vec3(0.3, 1, 1), vec3(1, 1, 0.3), vec3(1, 0.3, 1) };

vec3 getMaterial(float value)
{
	return materials[int(value * 7) % 7];
}
