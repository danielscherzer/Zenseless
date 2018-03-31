uniform mat4 gl_ModelViewProjectionMatrix;

void main() 
{
	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}