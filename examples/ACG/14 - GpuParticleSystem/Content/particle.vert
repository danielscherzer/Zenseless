#version 430 core

uniform float deltaTime;
uniform mat4x4 camera;
uniform float pointResolutionScale = 1;

out vec4 baseColor;

struct Particle
{
	vec3 position;
	float size; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
	vec3 velocity;
	uint color; //uint is aligned with previous vec3 to 16byte alignment, changing the order does not work
};

layout(std430) buffer BufferParticle
{
	Particle particle[];
};
 
bool outside(vec3 pos)
{
	return any(greaterThan(abs(pos), vec3(1))); 
}

void update(inout Particle p)
{
	p.position += p.velocity * deltaTime; //update particle buffer with new position

	if(outside(p.position))
	{
		p.velocity = -p.velocity; //bounce on walls
	}

	//set vertex outputs
	vec4 pos = camera * vec4(p.position, 1.0);
	gl_Position = pos;
	gl_PointSize = p.size / pos.z * pointResolutionScale; //points get smaller with distance
	baseColor = unpackUnorm4x8(p.color).xyza;
}

void main() 
{
	update(particle[gl_VertexID]);
}