#version 430 core	

uniform mat4 camera;
#ifdef SOLUTION
uniform mat4 light;
#endif

in vec4 position;
in vec3 normal;
in vec2 uv;

out blockData
{
#ifdef SOLUTION
	vec4 position_LS;
#endif
	vec3 normal;
	vec2 uv;
} o;

void main() 
{
#ifdef SOLUTION
	o.position_LS = light * position;
#endif
	o.normal = normal;
	o.uv = uv;
	gl_Position = camera * position;
}