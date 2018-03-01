#version 430 core

uniform sampler2D image;

in vec4 color;
in vec2 texCoord0; 

void main() 
{
	gl_FragColor = color * texture(image, texCoord0);
}