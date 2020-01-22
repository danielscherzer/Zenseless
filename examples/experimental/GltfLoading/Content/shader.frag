#version 430 core
uniform vec4 baseColor;
uniform vec3 cameraPos;

in Data
{
	vec3 normal;
	vec3 position;
	vec2 texCoord0;
} i;

out vec4 outputColor;

float lambert(vec3 light)
{
	return max(0.0, dot(i.normal, light));
}

float specular(vec3 toLight, vec3 v, float shininess)
{
	if(0.0 > dot(i.normal, toLight)) return 0;
	vec3 r = reflect(-toLight, i.normal);
	float cosRV = dot(r, v);
	if(0 > cosRV) return 0;
	return pow(cosRV, shininess);
}

vec4 hemisphericalLight(vec3 toLight)
{
	const vec3 skyColor = vec3(0.229,0.875,1.000);
	const vec3 groundColor = vec3(0.720,1.000,0.440);
	float w = 0.5 + 0.5 * dot(i.normal, toLight);
	return vec4(mix(groundColor, skyColor, w), 1.0);
}

vec4 metallic(vec3 light)
{
	vec3 v = normalize(cameraPos - i.position);
	float spec = specular(light, v, 8.0);
	return vec4(spec * vec3(1), 1.0);
}

void main() 
{
	vec3 toLight = normalize(vec3(0, 1, 0));
	vec4 color = baseColor ;//* vec4(i.texCoord0, 0, 0);
	outputColor = metallic(toLight) + 
		hemisphericalLight(toLight) * color;
}