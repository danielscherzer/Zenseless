#version 430 core

uniform MouseState
{
	vec2 position;
	int drawState;
};

uniform sampler2D oldObstacles;

in vec2 uv;
out vec4 color;

void main() 
{
	vec2 delta = (uv - position);
	vec2 normal;
	if(length(delta) < 0.05 && 0 != drawState)
	{
		normal = 1 == drawState ? normalize(delta) : vec2(0);
	}
	else
	{
		normal = texture(oldObstacles, uv).rg;
	}
	color = vec4(normal, 0, 0);
}