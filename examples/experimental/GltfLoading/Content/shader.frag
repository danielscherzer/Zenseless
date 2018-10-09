#version 430 core
uniform vec4 color;

in Data
{
	vec3 normal;
	vec3 position;
} i;

out vec4 outputColor;

void main() 
{
	vec3 light = vec3(10, 500, 10);
	float lambert = dot(i.normal, normalize(light - i.position));
	outputColor = lambert * color;
}