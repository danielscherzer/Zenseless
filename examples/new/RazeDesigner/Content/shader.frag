#version 430 compatibility
uniform vec4 color = vec4(1);
uniform sampler2D texStreet;

in vec2 texUV;
in float v;

out vec4 outputColor;

void main() 
{
	vec4 street = texture(texStreet, texUV);
	outputColor = mix(street, color, pow(abs(v), 4.0)); //spec of pow(x,y) says undefined if x < 0; NVIDIA does this, INTEL not
//	outputColor = street;
}