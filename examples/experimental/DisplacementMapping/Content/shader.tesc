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

vec2 toScreen(vec4 vec)
{
	vec4 pos = camera * vec;
	return pos.xy / pos.w;
}

// calculate edge tessellation level from two edge vertices in screen space
float calcEdgeTessellation(vec2 v0, vec2 v1)
{
	float d = distance(v0, v1);
	return clamp(lodScale * d, 1, 64);
}

bool outside(vec2 v)
{
	return greaterThan(abs(v), vec2(1));
}

void main()
{
	tcPos[gl_InvocationID] = gl_in[gl_InvocationID].gl_Position;
	tcTexCoord[gl_InvocationID] = i[gl_InvocationID].texCoord;
	instanceID = i[gl_InvocationID].instanceID;

	vec2 v0 = toScreen(gl_in[0].gl_Position);
	vec2 v1 = toScreen(gl_in[1].gl_Position);
	vec2 v2 = toScreen(gl_in[2].gl_Position);
	vec2 v3 = toScreen(gl_in[3].gl_Position);

	// check if every corner of patch is outside
	bvec4 outside = bvec4(outside(v0), outside(v1), outside(v2), outside(v3));
	if(all(outside))
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
		gl_TessLevelOuter[0] = calcEdgeTessellation(v3, v0);
		gl_TessLevelOuter[1] = calcEdgeTessellation(v0, v1);
		gl_TessLevelOuter[2] = calcEdgeTessellation(v1, v2);
		gl_TessLevelOuter[3] = calcEdgeTessellation(v2, v3);

		// calculate interior tessellation level - use average of outer levels
		gl_TessLevelInner[0] = 0.5 * (gl_TessLevelOuter[1] + gl_TessLevelOuter[3]);
		gl_TessLevelInner[1] = 0.5 * (gl_TessLevelOuter[0] + gl_TessLevelOuter[2]);
	}
}