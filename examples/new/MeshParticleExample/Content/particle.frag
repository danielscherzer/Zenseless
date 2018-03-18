#version 430 core
in vec3 baseColor;
in vec3 varNormal;

out vec4 color;

void main() 
{
	float lambert = min(1, dot(varNormal, normalize(vec3(1, 1, 0))) + 0.3);
	color = vec4(lambert * baseColor, 1);
}