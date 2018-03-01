#version 130

uniform mat4 gl_ModelViewProjectionMatrix;

in vec4 gl_Vertex;

out vec4 color;
out vec2 texCoord0;

void main() 
{
	color = gl_Color;
	texCoord0 = gl_MultiTexCoord0.xy;
	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}