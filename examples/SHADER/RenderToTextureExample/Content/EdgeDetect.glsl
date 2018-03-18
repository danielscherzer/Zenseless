const float PI = 3.14159265359;

uniform sampler2D image;
uniform float iGlobalTime;

in vec2 uv;

//sobel operator for x
mat3 sx = mat3( 
	1.0, 2.0, 1.0, 
	0.0, 0.0, 0.0, 
	-1.0, -2.0, -1.0 
	);

//sobel operator for y
mat3 sy = mat3( 
	1.0, 0.0, -1.0, 
	2.0, 0.0, -2.0, 
	1.0, 0.0, -1.0 
	);

float grayScale(vec3 color)
{
	return 0.2126 * color.r + 0.7152 * color.g + 0.0722 * color.b;
}

float convolve(mat3 a, mat3 b)
{
	return dot(a[0], b[0]) + dot(a[1], b[1]) + dot(a[2], b[2]);
}

void main()
{
	mat3 I;
	for (int i = 0; i < 3; ++i) 
	{
		for (int j = 0; j < 3; ++j) 
		{
			vec3 sample  = texelFetch(image, ivec2(gl_FragCoord) + ivec2(i - 1, j - 1), 0).rgb;
			I[i][j] = grayScale(sample);
		}
	}

	float gx = convolve(sx, I);
	float gy = convolve(sy, I);
	vec2 gxy = vec2(gx, gy);
	float g = sqrt(dot(gxy, gxy)); //sqrt(gx^2 + gy^2)
	gl_FragColor = vec4(vec3(g), 1.0);
}
