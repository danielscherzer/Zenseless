#version 430 core				

in vec3 instancePosition;

in vec3 position;
in vec3 normal;

out vec3 var_color;

void main() 
{
	var_color = normal;

	vec3 pos = position + instancePosition;
	gl_Position = vec4(pos, 1.0);
}