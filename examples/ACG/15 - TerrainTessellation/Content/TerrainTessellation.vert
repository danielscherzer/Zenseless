#version 420 core
uniform int columnCount = 5;

out Data
{
	flat int instanceID;
	vec2 texCoord;
} o;

// for each quad corner calculate vertex and texture coordinates
// texture coordinates are in the range [0, 1]^2 / columnCount for one quad so in the range [0, 1]^2 over all quad instances
// positions range from -vec2(columnCount / 2) to vec2(columnCount / 2) over all quad instances
void main() 
{
	const float halfeSize = 0.5;
	const vec2 vertices[4] = vec2[4](vec2(-halfeSize, -halfeSize),
		vec2( halfeSize, -halfeSize),
		vec2( halfeSize,  halfeSize),
		vec2(-halfeSize,  halfeSize));
	
	float x = gl_InstanceID % columnCount;
	float y = gl_InstanceID / columnCount;
	o.instanceID = gl_InstanceID;
	o.texCoord = (vertices[gl_VertexID] + vec2(1) + vec2(x, y)) / columnCount;

	vec2 pos = vertices[gl_VertexID] + vec2(x, y) - vec2(columnCount / 2);
	gl_Position = vec4(pos.x, 0, pos.y, 1.0);
}