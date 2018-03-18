#version 430 core
uniform sampler2D obstacles;

uniform Uniforms
{
	float random;
	float particleCount;
};

struct Particle
{
	vec2 position;
	vec2 velocity; //position + velocity are aligned to 16byte
};

float rnd()
{
//	return fract(gl_VertexID / particleCount * 1234567.89);
	return gl_VertexID / particleCount;
};

layout(std430) buffer Particles
{
	Particle particle[];
};

void update(inout Particle p)
{
	p.velocity += vec2(0, -0.0003);
	p.position += p.velocity; //update particle buffer with new position

	vec2 obstNormal = texture(obstacles, p.position * 0.5 + 0.5).xy;
	float speed = length(p.velocity);
	p.velocity = reflect(p.velocity, obstNormal);
	//p.velocity += obstNormal * speed * 0.1;

	float y = p.position.y;
	if(-1 > y)
	{
		p.position.y += 2;
		p.position.x = (rnd() * 2 - 1);
		p.velocity *= 0;
	}

	//set vertex outputs
	gl_Position = vec4(p.position, 0.5, 1.0);
	gl_PointSize = 1;
}

void main() 
{
	update(particle[gl_VertexID]);
}