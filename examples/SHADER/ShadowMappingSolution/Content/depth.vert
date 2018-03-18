#version 430 core	
			
uniform mat4 camera;

in vec4 position;

out blockData
{
	vec4 position;
} o;

void main() 
{
	o.position = camera * position;
	gl_Position = o.position;
}