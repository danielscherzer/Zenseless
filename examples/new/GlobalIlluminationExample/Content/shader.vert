#version 430 core	
			
struct Material //use 16 byte alignment or you have to query all variable offsets
{
	vec3 color;
	float shininess;
};

uniform bufferMaterials
{
	Material material[4];
};

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec2 uv;

out blockData
{
	vec3 position;
	vec3 normal;
	vec3 color;
	float reflectivity;
	float shininess;
} o;

void set(Material mat)
{
	o.color = mat.color;
	o.reflectivity = 0 == mat.shininess ? 0 : 1;
	o.shininess = 0 == mat.shininess ? 1 : mat.shininess;
}

void main() 
{
	o.position = position;
	o.normal = normal;
	set(material[int(uv.s)]);
	gl_Position = camera * vec4(position, 1.0);
}