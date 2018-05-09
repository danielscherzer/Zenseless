#version 430 core

uniform float deltaTime;
uniform mat4x4 camera;
uniform int particleCount;
uniform float pointResolutionScale = 1f;
uniform vec3 source = vec3(0);
uniform vec3 acceleration;
uniform float lifeTime = 10;

out vec4 baseColor;

struct Particle
{
	vec3 position;
	float size;
	vec3 velocity;
	float age;
};

layout(std430) buffer BufferParticle
{
	Particle particle[];
};
 
void update(inout Particle p)
{
	p.position += p.velocity * deltaTime; //update particle buffer with new position
	p.velocity += acceleration * deltaTime;
	p.age += deltaTime;

	//particle dead?
	if(p.age > lifeTime)
	{
		p.position = source;
//		p.velocity = 
		p.age = 0;
	}

	//set vertex outputs
	vec4 pos = camera * vec4(p.position, 1.0);
	gl_Position = pos;
	gl_PointSize = p.size / pos.z; //points get smaller with distance and are scaled with resolution
	baseColor = mix(vec4(1, 1, 1, 1), vec4(0, 0, 0, 0), p.age / lifeTime);
}

void main() 
{
	update(particle[gl_VertexID]);
}