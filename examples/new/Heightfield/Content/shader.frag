#version 430 core
uniform sampler2D texColor;
uniform sampler2D texStone;

in vec2 uvs;
out vec4 fragColor;

void main() 
{
	vec3 color = texture(texColor, uvs).rgb;
	vec3 stone = texture(texStone, uvs * 10.0).rgb;
	fragColor = vec4(1.0);
	fragColor = vec4(uvs, 0, 1);
//	fragColor = vec4(color, 1.0);
//	fragColor = vec4(stone * color, 1.0);
}