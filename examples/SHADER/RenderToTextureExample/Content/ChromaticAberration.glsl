uniform sampler2D image;

in vec2 uv;

void main () {
	vec2 xyOffsetScale = vec2(0.003);
	vec2 rOffset = xyOffsetScale * vec2(0, 0);
	vec2 gOffset = xyOffsetScale * vec2(1, 1);
	vec2 bOffset = xyOffsetScale * vec2(2, 2);
    float r = texture2D(image, uv - rOffset).r;  
    float g = texture2D(image, uv - gOffset).g;
    float b = texture2D(image, uv - bOffset).b;  
    // Combine the offset colors.
    gl_FragColor = vec4(r, g, b, 1.0);
}
