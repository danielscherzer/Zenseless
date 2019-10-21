#version 430 core
uniform sampler2D texDiffuse;

in vec3 n;

#ifdef SOLUTION
	in vec2 toFrag_uv;
#endif

out vec4 color;

void main() 
{
	//use normal as color
	color = vec4(n, 1.0);
#ifdef SOLUTION
	color = texture(texDiffuse, toFrag_uv);
#endif
}