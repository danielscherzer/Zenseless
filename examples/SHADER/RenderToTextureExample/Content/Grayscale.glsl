#version 430 core

uniform sampler2D image;

in vec2 uv;

float grayScale(vec3 color)
{
	return 0.2126 * color.r + 0.7152 * color.g + 0.0722 * color.b;
}

void main() 
{
	vec3 color = texture(image, uv).rgb;
	color = vec3(grayScale(color));
	gl_FragColor = vec4(color, 1.0);
}