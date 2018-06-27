#version 420 core
uniform int instanceSqrt = 5;

out Data
{
	flat int instanceID;
	vec2 texCoord;
} o;

void main() 
{
	const float size = 0.5;
	const vec2 vertices[4] = vec2[4](vec2(-size, -size),
		vec2( size, -size),
		vec2( size,  size),
		vec2(-size,  size));
	
	float x = gl_InstanceID % instanceSqrt;
	float y = gl_InstanceID / instanceSqrt;
	o.instanceID = gl_InstanceID;
	o.texCoord = (vertices[gl_VertexID] + vec2(1) + vec2(x, y)) / instanceSqrt;

	vec2 pos = vertices[gl_VertexID] + vec2(x, y) - vec2(instanceSqrt / 2);

	gl_Position = vec4(pos.x, 0, pos.y, 1.0);
}