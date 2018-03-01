#version 430 core

uniform sampler2D image;

in vec2 uv;

void main() 
{
	gl_FragColor = texture(image, uv);
}
