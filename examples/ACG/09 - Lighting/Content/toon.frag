#version 430 core
uniform vec3 cameraPosition;

uniform vec4 materialColor;
uniform vec4 ambientLightColor;
uniform vec3 light2Position;
uniform vec4 light2Color;

in vec3 pos;
in vec3 n;

out vec4 color;

#ifdef SOLUTION
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
#endif 

void main() 
{
	color = vec4(n, 1);

#ifdef SOLUTION
	vec3 normal = normalize(n);
	vec3 v = normalize(cameraPosition - pos);

	//ambient lighting
	vec4 ambient = ambientLightColor * materialColor;

	//point light
	vec3 l = normalize(light2Position - pos);
	float diff = lambert(normal, l);

	//toon shading == discrete (quantized) steps of diffuse lighting
	vec4 maxColor = materialColor * light2Color;
	color = (diff > 0.9) ? maxColor : (diff > 0.5) ? 0.5 * maxColor : ambient;

	float spec = specular(normal, l, v, 100);
	if(spec > 0.8) color = vec4(1);

	//cel shading == detect edges and color them
	if(abs(dot(normal, v)) < 0.18)
	{
        color = vec4(0, 0, 1, 1);
    }
#endif
}