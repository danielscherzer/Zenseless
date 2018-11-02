#version 430 core
const int MAX_BONES = 100;
uniform mat4 u_jointMat[100];
uniform mat4 camera;
uniform mat4 world;

in vec4 position;
in vec3 normal;
in vec4 joints_0;
in vec4 weights_0;
in vec2 texcoord_0;

out Data
{
	vec3 normal;
	vec3 position;
	vec2 texCoord0;
} o;

void main() 
{
	mat4 skinMat = weights_0.x * u_jointMat[int(joints_0.x)];
	skinMat += weights_0.y * u_jointMat[int(joints_0.y)];
	skinMat += weights_0.z * u_jointMat[int(joints_0.z)];
	skinMat += weights_0.w * u_jointMat[int(joints_0.w)];

	mat4 worldSkin = world * skinMat;
	o.normal = mat3(worldSkin) * normal;
	vec4 position_world = worldSkin * position;
	o.position = position_world.xyz;
	o.texCoord0 = texcoord_0;
	gl_Position = camera * position_world;
}
