#version 430 core

const float TWO_PI = 3.1415926535897932384626433832795 * 2.0;

// from https://iquilezles.org/www/articles/palettes/palettes.htm
vec3 color(float t)
{
	vec3 a = vec3(0.5, 0.5, 0.5);
	vec3 b = vec3(0.5, 0.5, 0.5);
	vec3 c = vec3(0.6, 0.6, 0.6);
	vec3 d = vec3(0.1, 0.2, 0.3);
	return a + b * cos(TWO_PI * (c * t + d));
}


uniform sampler2D texParticle;
in float fade;

out vec4 fragColor;

void main() 
{
	vec4 diffColor = texture(texParticle, gl_PointCoord);
	diffColor.rgb *= color(1.0 - fade);
	diffColor.a *= fade; // fade out
	fragColor = diffColor;
}