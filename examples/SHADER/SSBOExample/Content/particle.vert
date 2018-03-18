#version 430 core

uniform float deltaTime;
uniform int particleCount;

out vec3 baseColor;

struct Particle
{
	vec2 position;
	vec2 velocity; //position + velocity are aligned to 16byte
	vec3 color;
	float size; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
};

layout(std430) buffer BufferParticle
{
	Particle particle[];
};
 
bool outside(vec2 pos)
{
	return any(greaterThan(abs(pos), vec2(1))); 
}

void update(inout Particle p)
{
	p.position += p.velocity * deltaTime; //update particle buffer with new position

	if(outside(p.position))
	{
		p.velocity = -p.velocity; //bounce on walls
	}

	//set vertex outputs
	gl_PointSize = p.size;
	baseColor = p.color;
	gl_Position = vec4(p.position, 0.5, 1.0);
}

void main() 
{
	update(particle[gl_VertexID]);
}