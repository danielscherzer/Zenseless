#version 430 core

uniform sampler2D image;

in vec4 color;
in vec2 texCoord0;

out vec4 fragColor;
void main() 
{
	fragColor = color * texture(image, texCoord0);
}