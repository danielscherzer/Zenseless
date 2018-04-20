#version 430 core
uniform vec3 cameraPosition;

uniform vec4 materialColor;

uniform vec4 ambientLightColor;

uniform vec3 light1Direction;
uniform vec4 light1Color;

uniform vec3 light2Position;
uniform vec4 light2Color;

uniform vec3 light3Position;
uniform vec3 light3Direction;
uniform float light3Angle;
uniform vec4 light3Color;

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
		if(0 > dot(n, l)) return 0;
		vec3 r = reflect(-l, n);
		float cosRV = dot(r, v);
		if(0 > cosRV) return 0;
		return pow(cosRV, shininess);
	}
#endif

void main() 
{
	vec3 normal = normalize(n);

	color =  vec4(abs(normal), 1);

#ifdef SOLUTION
	vec3 v = normalize(cameraPosition - pos);
	//ambient lighting
	vec4 ambient = ambientLightColor * materialColor;

	//directional light
	vec4 light1 = materialColor * light1Color * lambert(normal, -light1Direction);
				 light1Color * specular(normal, -light1Direction, v, 100);

	//point light
	vec3 light2l = normalize(light2Position - pos);
	vec4 light2 = 
	materialColor * light2Color * lambert(normal, light2l) + 
	light2Color * specular(normal, light2l, v, 64);

	//spot light
	vec4 light3 = vec4(0);
	vec3 light3l = normalize(light3Position - pos);
	if(acos(dot(light3l, -light3Direction)) < light3Angle)
	{
		light3 = materialColor * light3Color * lambert(normal, light3l)
				+ light3Color * specular(normal, light3l, v, 100);
	}

	//combine
	color = ambient	+ light1 + light2 + light3;
#endif
}