#version 430 core

uniform sampler2D image;

in vec2 uv;

void main () {
	vec2 xyOffsetScale = vec2(0.003);
	vec2 rOffset =  1 * xyOffsetScale;
	vec2 gOffset = -1 * xyOffsetScale;
	vec2 bOffset =  2 * xyOffsetScale;
    float r = texture2D(image, uv - rOffset).r;
    float g = texture2D(image, uv - gOffset).g;
    float b = texture2D(image, uv - bOffset).b;
    // Combine the offset colors.
    gl_FragColor = vec4(r, g, b, 1.0);
}
