#version 430 core
uniform sampler2D texDiffuse;

in vec3 n;
in vec2 uvs;
out vec4 color;

void main() 
{
	//use normal as color
	color = vec4(n, 1.0);
	//color = texture(texDiffuse, uvs);
}