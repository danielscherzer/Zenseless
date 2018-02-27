#version 130

uniform vec2 iResolution;

out vec2 uv; 
out vec2 fragCoord;

void main() 
{
	const vec2 vertices[4] = vec2[4](vec2(-1.0, -1.0),
		vec2( 1.0, -1.0),
		vec2( 1.0,  1.0),
		vec2(-1.0,  1.0));

	vec2 pos = vertices[gl_VertexID];
	uv = pos * 0.5 + 0.5;
	fragCoord = uv * iResolution;
	gl_Position = vec4(pos, 0.0, 1.0);
}