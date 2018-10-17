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
	vec3 light2 = normalize(vec3(-1, 0, -1));
	float lambert = dot(i.normal, normalize(light - i.position));
	outputColor = (lambert + dot(i.normal, light2) + 0.3) * 
		color;
}