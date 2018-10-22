#version 430 core
uniform mat4 camera;
uniform mat4 world;
uniform mat4 u_jointMat[] = { mat4(1.0), mat4(1.0), mat4(1.0), mat4(1.0) };

in vec4 position;
in vec3 normal;
in vec4 joints_0;
in vec4 weights_0;

out Data
{
	vec3 normal;
	vec3 position;
} o;

void main() 
{
    mat4 skinMat =
        weights_0.x * u_jointMat[int(joints_0.x)] +
        weights_0.y * u_jointMat[int(joints_0.y)] +
        weights_0.z * u_jointMat[int(joints_0.z)] +
        weights_0.w * u_jointMat[int(joints_0.w)];

	mat4 worldSkin = world * skinMat;
	o.normal = mat3(worldSkin) * normal;
	vec4 position_world = worldSkin * position;
	o.position = position_world.xyz;
	gl_Position = camera * position_world;
}