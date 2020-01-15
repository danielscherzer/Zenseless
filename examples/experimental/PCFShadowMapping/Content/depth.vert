#version 430 core	
			
uniform mat4 camera;

in vec4 position;

#ifdef SOLUTION
out blockData
{
	vec4 position;
} o;
#endif

void main() 
{
	gl_Position = camera * position;
#ifdef SOLUTION
	o.position = camera * position;
	gl_Position = o.position;
#endif
}