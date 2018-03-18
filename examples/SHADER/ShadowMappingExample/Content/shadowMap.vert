#version 430 core	
			
uniform mat4 camera;

in vec4 position;
in vec3 normal;
in vec2 uv;

out blockData
{
	vec3 normal;
	vec2 uv;
} o;

void main() 
{
	o.normal = normal;
	o.uv = uv;
	gl_Position = camera * position;
}