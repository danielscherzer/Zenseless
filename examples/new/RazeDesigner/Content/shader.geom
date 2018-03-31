#version 330 core
#define OUTPUT_COUNT 20
#define OUTPUT_COUNT2 40

layout(lines_adjacency) in;
layout(line_strip, max_vertices = OUTPUT_COUNT) out;
//layout(triangle_strip, max_vertices = OUTPUT_COUNT2) out;

float H1(float t)
{
	return 2 * t * t * t - 3 * t * t + 1;
}

float H2(float t)
{
	return -2 * t * t * t + 3 * t * t;
}

float H3(float t)
{
	return t * t * t - 2 * t * t + t;
}

float H4(float t)
{
	return t * t * t - t * t;
}

vec2 Tangent(vec2 tangent0, vec2 tangent1, float t)
{
	return H3(t) * tangent0 + H4(t) * tangent1;
}

vec2 EvaluateSegment(vec2 point0, vec2 point1, vec2 tangent0, vec2 tangent1, float t)
{
	return H1(t) * point0 + H2(t) * point1 + H3(t) * tangent0 + H4(t) * tangent1;
}

void Emit(vec2 p)
{
	gl_Position = vec4(p, 0, 1);
	EmitVertex();
}

void CreateStrip(vec2 p, vec2 tangent)
{
	vec2 dir = normalize(tangent);
	vec2 normal = dir.yx;
	normal.y = -normal.y;
	float width = 0.1;
	Emit(p + width * normal);
	Emit(p - width * normal);
}

void main() 
{
	vec2 p0 = gl_in[0].gl_Position.xy;
	vec2 p1 = gl_in[1].gl_Position.xy;
	vec2 p2 = gl_in[2].gl_Position.xy;
	vec2 p3 = gl_in[3].gl_Position.xy;
	vec2 t1 = 0.5 * (p2 - p0);
	vec2 t2 = 0.5 * (p3 - p1);
	for (int i = 0; i < OUTPUT_COUNT; ++i)
	{
		float t = i;
		t /= OUTPUT_COUNT - 1;
		vec2 pos = EvaluateSegment(p1, p2, t1, t2, t);
		Emit(pos);
//		CreateStrip(pos, Tangent(t1, t2, t));
	}

	EndPrimitive();
}