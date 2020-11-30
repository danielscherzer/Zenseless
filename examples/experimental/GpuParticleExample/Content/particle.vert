#version 430 core

//from https://www.shadertoy.com/view/4djSRW
vec3 hash31(float p)
{
	vec3 p3 = fract(vec3(p) * vec3(.1031, .1030, .0973));
	p3 += dot(p3, p3.yzx + 33.33);
	return fract((p3.xxy + p3.yzz) * p3.zyx); 
}

uniform mat4x4 camera;
uniform float pointResolutionScale = 1.0;

uniform vec3 source = vec3(0.0);
uniform float deltaTime;
uniform vec3 acceleration;
uniform float lifeTime = 1.0;
uniform vec3 resetVelocityLowerBounds = vec3(0.0);
uniform vec3 resetVelocityUpperBounds = vec3(1.0);

out float fade;

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
	p.velocity += acceleration * deltaTime;
	p.position += p.velocity * deltaTime; //update particle buffer with new position
	p.age += deltaTime;

	//particle dead?
	if(p.age > lifeTime)
	{
		p.position = source;
		vec3 hash = hash31(gl_VertexID); // each vertex needs its own hash
		p.velocity = mix(resetVelocityLowerBounds, resetVelocityUpperBounds, hash);
		p.age = 0.0;
	}

	//set vertex outputs
	vec4 pos = camera * vec4(p.position, 1.0);
	gl_Position = pos;
	gl_PointSize = p.size / pos.z; //points get smaller with distance and are scaled with resolution
	fade = 1.0 - p.age / lifeTime;
}

void main() 
{
	update(particle[gl_VertexID]);
}