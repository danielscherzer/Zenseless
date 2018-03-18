#version 430 core	
			
uniform mat4 camera;
uniform mat4 light;

in vec4 position;
in vec3 normal;
in vec2 uv;

out blockData
{
	vec4 position_LS;
	vec3 normal;
	vec2 uv;
} o;

void main() 
{
	o.position_LS = light * position;
	o.normal = normal;
	o.uv = uv;
	gl_Position = camera * position;
}