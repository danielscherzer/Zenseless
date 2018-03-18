#version 330

uniform sampler2D offScreenTexture;
uniform float effectScale = 0.3;

in vec2 uv;

float circle(vec2 coord, float startFadeOut, float endFadeOut)
{
	float dist = distance(vec2(0.5), coord);
	return 1.0 - smoothstep(startFadeOut, endFadeOut, dist);
}

void main() {
	vec3 color = texture(offScreenTexture, uv).rgb;
	
	float startFadeOut = 0.6 - clamp(effectScale, 0.0, 1.0);
	float smoothness = 0.5;
	color *= circle(uv, startFadeOut, startFadeOut + smoothness);

	gl_FragColor = vec4(color, 1.0);
}