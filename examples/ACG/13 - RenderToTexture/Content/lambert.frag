#version 430 core

in Data
{
	vec3 position;
	vec3 normal;
	flat uint material;
} inData;

out vec4 color;

void main() 
{
	const vec3 materials[] = { vec3(1), vec3(1, 0, 0), vec3(0, 1, 0), vec3(0, 1, 1) };

	vec3 light = normalize(vec3(1, 0.5, 1));
	vec3 normal = normalize(inData.normal);
	vec3 albedo = materials[inData.material];

	vec3 ambient = 0.3 * albedo;
	vec3 diffuse = max(0, dot(normal, light)) * albedo;

	color =  vec4(ambient + diffuse, 1);
}