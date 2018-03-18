#version 430 core

uniform float effectScale = 0.3;
uniform sampler2D offScreenTexture;

in vec2 uv;

float grayScale(vec3 color) 
{
  vec3 weight = vec3(0.2126, 0.7152, 0.0722); 
  return dot(color, weight);
}

out vec4 color; 

void main() 
{
  vec3 tex = texture(offScreenTexture, uv).rgb;
  color = vec4(vec3(grayScale(tex)), 1);
}
