#version 430 core
uniform sampler2D texParticle;

in float fadeFrag;

out vec4 color;

void main() 
{
	//color = vec4(gl_PointCoord, 0, 1);
	color = texture(texParticle, gl_PointCoord);
	color.a *=  fadeFrag; //fade out 
	color.rgb = 0.3 * vec3(0.2, 0.2, 0.4);
}