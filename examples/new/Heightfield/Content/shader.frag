#version 430 core
uniform sampler2D texColor;
uniform sampler2D texHeightfield;

in vec2 uvs;
out vec4 color;

void main() 
{
	color = vec4(1.0);
	float height = texture(texHeightfield, uvs).r;
	color = texture(texColor, uvs);
}