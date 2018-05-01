#version 430 core

const float PI = 3.14159265359;

uniform sampler2D image;
uniform float effectScale = 0.3;

in vec2 uv;

mat3 identity = mat3( 
	0.0, 0.0, 0.0, 
	0.0, 1.0, 0.0, 
	0.0, 0.0, 0.0 
	);

mat3 sharpen = mat3( 
	0.0, -1.0, 0.0, 
	-1.0, 5.0, -1.0, 
	0.0, -1.0, 0.0 
	);

mat3 blur = mat3( 
	1.0 / 9, 1.0 / 9, 1.0 / 9,
	1.0 / 9, 1.0 / 9, 1.0 / 9,
	1.0 / 9, 1.0 / 9, 1.0 / 9
	);

mat3 edgeDetection = mat3( 
	0.0, 1.0, 0.0,
	1.0, -4.0, 1.0,
	0.0, 1.0, 0.0
	);

mat3 edgeDetection2 = mat3( 
	-1.0, -1.0, -1.0,
	-1.0, 8.0, -1.0,
	-1.0, -1.0, -1.0
	);

void main()
{
	vec3 color = vec3(0);
	for (int i = 0; i < 3; ++i) 
	{
		for (int j = 0; j < 3; ++j) 
		{
			vec3 aSample = texelFetch(image, ivec2(gl_FragCoord) + ivec2(i - 1, j - 1), 0).rgb;
			color += edgeDetection2[j][i] * aSample;
		}
	}
	//color /= 9;

	gl_FragColor = vec4(color, 1.0);
}
