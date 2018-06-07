#version 430 core

#ifdef SOLUTION
uniform sampler2D texShadowMap;
uniform vec3 lightPosition;
#endif
uniform vec3 ambient;

in blockData
{
#ifdef SOLUTION
	vec4 position_LS;
	vec3 position;
#endif
	vec3 normal;
} i;

#ifdef SOLUTION
float lambert(vec3 n, vec3 l)
{
	return max(0, dot(n, l));
}
#endif

out vec4 color;

void main() 
{
	color = vec4(i.normal, 1);
#ifdef SOLUTION
	vec3 coord = i.position_LS.xyz / i.position_LS.w;
	float depth = texture(texShadowMap, coord.xy * 0.5 + 0.5).r;
	vec3 lighting = ambient;
	if(depth + 0.001 > coord.z)
	{
		vec3 toLight = normalize(lightPosition - i.position);
		lighting += lambert(normalize(i.normal), toLight) * vec3(1);
	}
	color = vec4(lighting, 1);
#endif
}