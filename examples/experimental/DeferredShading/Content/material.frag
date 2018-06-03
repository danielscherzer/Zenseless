#version 430 core

in Data
{
	vec3 position;
	vec3 normal;
	flat float material;
} inData;

out vec4 normalMaterial;
out vec3 position;

void main() 
{
	normalMaterial =  vec4(normalize(inData.normal), inData.material);
	position = inData.position;
}