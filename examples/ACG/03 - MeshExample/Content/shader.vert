#version 430 core
in vec3 position;
in vec3 normal;
in vec2 uv;

out vec3 n;
#ifdef SOLUTION
	out vec2 toFrag_uv;
#endif

void main() 
{
	n = normal;
#ifdef SOLUTION
	toFrag_uv = uv;
#endif
	gl_Position = vec4(position, 1.0);
}