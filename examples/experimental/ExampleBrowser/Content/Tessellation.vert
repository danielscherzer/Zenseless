#version 420 core

void main() 
{
	const float size = 0.75;
	const vec2 vertices[4] = vec2[4](vec2(-size, -size),
		vec2( size, -size),
		vec2( size,  size),
		vec2(-size,  size));
	
	vec2 pos = vertices[gl_VertexID];
	gl_Position = vec4(pos, 0.0, 1.0);
}