#version 430 core

in vec3 v_pos;
in vec3 v_n;
in vec2 v_uv;

out vec4 color;

void main() 
{
	vec3 light = normalize(vec3(1, 0.5, 1));
	vec3 normal = normalize(v_n);

	float lambert = max(0.2, dot(normal, light));
	vec3 diffuse = vec3(v_n) * lambert;

	color =  vec4(diffuse, 1);
}