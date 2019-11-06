uniform sampler2D image;

in vec2 uv;

float circle(vec2 coord, float startFadeOut, float endFadeOut)
{
	float dist = length(vec2(0.5) - coord);
	return 1 - smoothstep(startFadeOut, endFadeOut, dist);
}

void main() {
	vec3 color = texture(image, uv).rgb;

	color *= circle(uv, 0.3, 0.8);
		
	gl_FragColor = vec4(color, 1.0);
}