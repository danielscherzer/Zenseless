#version 430 core

uniform sampler2D image;
uniform float iGlobalTime;
uniform float amplitude = 0.01;
uniform float frequency = 15;
uniform float speed = 10;

in vec2 uv;

void main() 
{
	// range [0..1]^2 -> [-1..1]^2
	vec2 range11 = 2 * uv - 1;

	float radius = length(range11); // distance to center
	float ripple = abs(sin(radius * frequency - speed * iGlobalTime));

	vec2 newUv = uv + ripple * amplitude; //distort uv by ripple
	
	vec3 color = texture(image, newUv).rgb;
	gl_FragColor = vec4(color, 1.0);
}
