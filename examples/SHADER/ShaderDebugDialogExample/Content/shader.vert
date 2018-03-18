#version 430 core				

out vec3 pos;

void main() {
    const vec2 vertices[4] = vec2[4](vec2(-0.6, -0.8),
                                    vec2( 0.9, -0.9),
                                    vec2( 0.2,  0.8),
                                    vec2(-0.9,  0.9));
    pos = vec3(vertices[gl_VertexID], 0.5);
    gl_Position = vec4(pos, 1.0);
}
