#version 430 core

#ifdef SOLUTION
in blockData
{
	vec4 position;
} i;
#endif

out vec4 color;

void main() 
{
	color = vec4(1);
#ifdef SOLUTION
	color = vec4(i.position.z / i.position.w);
#endif
}