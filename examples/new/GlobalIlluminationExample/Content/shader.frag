#version 430 core
uniform sampler2D texMaterial;

uniform vec3 ambient;
uniform vec3 lightPosition;
uniform vec3 lightColor;
uniform vec3 cameraPosition;

in blockData
{
	vec3 position;
	vec3 normal;
	vec3 color;
	float reflectivity;
	float shininess;
} i;

float lambert(vec3 n, vec3 l)
{
	return max(0, dot(n, l));
}

float specular(vec3 n, vec3 l, vec3 v, float shininess)
{
	//if(0 > dot(n, l)) return 0;
	vec3 r = reflect(-l, n);
	return pow(max(0, dot(r, v)), shininess);
}

out vec4 color;

void main() 
{
	vec3 normal = normalize(i.normal);
	vec3 v = normalize(cameraPosition - i.position);
	vec3 l = normalize(lightPosition - i.position);

	//point light
	vec3 light = i.color * (ambient + lightColor * lambert(normal, l))
				+ i.reflectivity * lightColor * specular(normal, l, v, i.shininess)
				;


	color =  vec4(light, 1);
}