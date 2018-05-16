#version 430 core

uniform sampler2D image;

in vec2 uv;

void main() 
{
	vec3 image = texture(image, uv).rgb;

	float sepiaMix = dot(vec3(0.3, 0.59, 0.11), image); 
	vec3 sepia = mix(vec3( 0.2, 0.05, 0.0), vec3( 1.0, 0.9, 0.5), sepiaMix);
	gl_FragColor = vec4(mix(image, sepia, 0.7), 1.0);
}
