#version 430 compatibility
#define OUTPUT_COUNT 80

layout(lines_adjacency) in;
layout(triangle_strip, max_vertices = OUTPUT_COUNT) out;

float H1(float t) { return 2 * t * t * t - 3 * t * t + 1; }
float H2(float t) { return -2 * t * t * t + 3 * t * t; }
float H3(float t) { return t * t * t - 2 * t * t + t; }
float H4(float t) { return t * t * t - t * t; }

vec2 EvaluateSegment(vec2 point0, vec2 point1, vec2 tangent0, vec2 tangent1, float t)
{
	return H1(t) * point0 + H2(t) * point1 + H3(t) * tangent0 + H4(t) * tangent1;
}

float dH1(float t) { return 6 * t * t - 6 * t; }
float dH2(float t) { return -6 * t * t + 6 * t; }
float dH3(float t) { return 3 * t * t - 4 * t + 1; }
float dH4(float t) { return 3 * t * t - 2 * t; }

vec2 Tangent(vec2 point0, vec2 point1, vec2 tangent0, vec2 tangent1, float t)
{
	return dH1(t) * point0 + dH2(t) * point1 + dH3(t) * tangent0 + dH4(t) * tangent1;
}

out vec2 texUV;
out float v;

uniform float aspect;

void Emit(vec2 p)
{
	texUV = p;
	gl_Position = gl_ModelViewProjectionMatrix * vec4(p, 0, 1);
	EmitVertex();
}

void CreateStrip(vec2 p, vec2 tangent)
{
	vec2 dir = normalize(tangent);
	vec2 normal = dir.yx;
	normal.y = -normal.y;
	float width = 0.1;
	v = 1.0;
	Emit(p + width * normal);
	v = -1.0;
	Emit(p - width * normal);
}

void main() 
{
	vec2 p0 = gl_in[0].gl_Position.xy;
	vec2 p1 = gl_in[1].gl_Position.xy;
	vec2 p2 = gl_in[2].gl_Position.xy;
	vec2 p3 = gl_in[3].gl_Position.xy;
	vec2 t1 = 0.5 * (p2 - p0); // central difference
	vec2 t2 = 0.5 * (p3 - p1); // central difference
	const int steps = OUTPUT_COUNT / 2;
	const float deltaT = 1.0 / (steps - 1);
	for (int i = 0; i < steps; ++i)
	{
		float t = i * deltaT;
		vec2 pos = EvaluateSegment(p1, p2, t1, t2, t);
		vec2 tangent = Tangent(p1, p2, t1, t2, t);
		CreateStrip(pos, tangent);
	}
	EndPrimitive();
}