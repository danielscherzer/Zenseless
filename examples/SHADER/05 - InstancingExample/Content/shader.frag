#version 430 core
in vec3 baseColor;
in vec3 varNormal;

out vec4 color;

void main() 
{
	color = vec4(baseColor, 1.0);
#ifdef SOLUTION
	float lambert = max(0, dot(varNormal, normalize(vec3(1, 1, 0))) + 0.3);
	color = vec4(lambert * baseColor, 1);
#endif
}