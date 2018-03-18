const float PI = 3.14159265359;

uniform sampler2D image;
uniform float iGlobalTime;

in vec2 uv;

float func(float x)
{
	return sign(sin(x)) * pow(sin(x), 3.0);
}

void main () {
	// range [-1..1]²
    vec2 range11 = 2 * uv - 1;

	//cartesian to polar coordinates
    float radius = length(range11); // radius of current pixel
    float angle = atan(range11.y, range11.x); //angel of current pixel [-PI..PI] 

	//distort angle
	float amplitude = 7.5;
	float frequency = 0.05;
	float startOffset = 0.5;

	float newAngle = angle + amplitude * (radius + startOffset) * func(radius * frequency + iGlobalTime);

	//polar to cartesian
	float x = radius * cos(newAngle);
	float y = radius * sin(newAngle);

	vec2 newUv = (vec2(x, y) + 1) * 0.5;
	
	vec3 color = texture(image, newUv).rgb;  
    gl_FragColor = vec4(color, 1.0);
}
