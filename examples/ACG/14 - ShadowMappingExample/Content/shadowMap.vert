#version 430 core	

uniform mat4 camera;
#ifdef SOLUTION
uniform mat4 light;
#endif

in vec4 position;
in vec3 normal;

out blockData
{
#ifdef SOLUTION
	vec4 position_LS;
	vec3 position;
#endif
	vec3 normal;
} o;

void main() 
{
#ifdef SOLUTION
	o.position = position.xyz;
	o.position_LS = light * position;
#endif
	o.normal = normal;
	gl_Position = camera * position;
}