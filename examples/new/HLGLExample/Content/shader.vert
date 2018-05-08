#version 430 core

uniform Uniforms
{
	mat4 camera;
};

in vec3 translate;

in vec3 position;
in vec3 normal;
in vec2 uv;

out vec3 n;
out vec2 uvs;

void main() 
{
	n = normal;
	uvs = uv;
	vec3 trans = vec3(0);
//	vec3 trans = translate.xyz;
//	vec3 trans = instanceTranslate[gl_InstanceID].xyz;
	gl_Position = camera * vec4(translate + position, 1.0);
}