#version 420 core
layout (vertices = 4) out;

uniform mat4 camera;
uniform float lodScale = 100; // distance scaling variable

in Data
{
	flat int instanceID;
	vec2 texCoord;
} i[];

patch out int instanceID;

out vec4 tcPos[];
out vec2 tcTexCoord[];

// calculate edge tessellation level from two edge vertices in screen space
float calcEdgeTessellation(vec2 v0, vec2 v1)
{
	float d = distance(v0, v1);
	return clamp(lodScale * d, 1, 64);
}

bool test(vec3 v[4], int axis)
{
	vec4 coords = vec4(v[0][axis], v[1][axis], v[2][axis], v[3][axis]);
	bool right = all(greaterThan(coords, vec4(1)));
	if(right) return true;
	bool left = all(lessThan(coords, vec4(-1)));
	if(left) return true;
	return false;
}

bool outside(vec3 v[4])
{
	if(test(v, 0)) return true;
	if(test(v, 2)) return true;
	return false;
}

vec3 toScreen(vec4 vec)
{
	vec4 pos = camera * vec;
	return pos.xyz / pos.w;
}

void main()
{
	tcPos[gl_InvocationID] = gl_in[gl_InvocationID].gl_Position;
	tcTexCoord[gl_InvocationID] = i[gl_InvocationID].texCoord;
	instanceID = i[gl_InvocationID].instanceID;

	vec3 v[4];
	v[0] = toScreen(gl_in[0].gl_Position);
	v[1] = toScreen(gl_in[1].gl_Position);
	v[2] = toScreen(gl_in[2].gl_Position);
	v[3] = toScreen(gl_in[3].gl_Position);

	// check if patch is outside
	if(outside(v))
	{
		// culling
		gl_TessLevelOuter[0] = -1;
		gl_TessLevelOuter[1] = -1;
		gl_TessLevelOuter[2] = -1;
		gl_TessLevelOuter[3] = -1;
		gl_TessLevelInner[0] = -1;
		gl_TessLevelInner[1] = -1;
	}
	else
	{
		gl_TessLevelOuter[0] = calcEdgeTessellation(v[3].xy, v[0].xy);
		gl_TessLevelOuter[1] = calcEdgeTessellation(v[0].xy, v[1].xy);
		gl_TessLevelOuter[2] = calcEdgeTessellation(v[1].xy, v[2].xy);
		gl_TessLevelOuter[3] = calcEdgeTessellation(v[2].xy, v[3].xy);

		// calculate interior tessellation level - use average of outer levels
		gl_TessLevelInner[0] = 0.5 * (gl_TessLevelOuter[1] + gl_TessLevelOuter[3]);
		gl_TessLevelInner[1] = 0.5 * (gl_TessLevelOuter[0] + gl_TessLevelOuter[2]);
	}
}