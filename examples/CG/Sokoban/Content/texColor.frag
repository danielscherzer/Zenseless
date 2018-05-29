#version 430 core
uniform vec4 tint = vec4(1);
uniform sampler2DArray texArray;

in vec2 uv;
in float tileType;

out vec4 fragColor;

void main() 
{
	fragColor = tint * texture(texArray, vec3(uv, tileType));
}