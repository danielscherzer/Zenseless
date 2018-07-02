#version 420 core

uniform mat4 camera;
uniform sampler2D texDisplacement;
uniform int instanceSqrt = 5;

layout (quads, fractional_odd_spacing, ccw) in;

out Data
{
	flat int instanceID;
	vec3 normal;
} o;

in vec4 tcPos[gl_MaxPatchVertices];
in vec2 tcTexCoord[gl_MaxPatchVertices];

patch in int instanceID;

vec2 interpolate(vec2 v1, vec2 v2, vec2 v3, vec2 v4)
{
	vec2 aX = mix(v1, v2, gl_TessCoord.x);
	vec2 bX = mix(v4, v3, gl_TessCoord.x);
	return mix(aX, bX, gl_TessCoord.y);
}

vec4 interpolate(vec4 v1, vec4 v2, vec4 v3, vec4 v4)
{
	vec4 aX = mix(v1, v2, gl_TessCoord.x);
	vec4 bX = mix(v4, v3, gl_TessCoord.x);
	return mix(aX, bX, gl_TessCoord.y);
}

vec2 hash( in vec2 x )  // replace this by something better
{
    const vec2 k = vec2( 0.3183099, 0.3678794 );
    x = x*k + k.yx;
    return -1.0 + 2.0*fract( 16.0 * k*fract( x.x*x.y*(x.x+x.y)) );
}

// The MIT License
// Copyright 2017 Inigo Quilez
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// return gradient noise (in x) and its derivatives (in yz)
vec3 noised( in vec2 p )
{
	vec2 i = floor( p );
	vec2 f = fract( p );

	// cubic interpolation
	vec2 u = f*f*(3.0-2.0*f);
	vec2 du = 6.0*f*(1.0-f);

	vec2 ga = hash( i + vec2(0.0,0.0) );
	vec2 gb = hash( i + vec2(1.0,0.0) );
	vec2 gc = hash( i + vec2(0.0,1.0) );
	vec2 gd = hash( i + vec2(1.0,1.0) );

	float va = dot( ga, f - vec2(0.0,0.0) );
	float vb = dot( gb, f - vec2(1.0,0.0) );
	float vc = dot( gc, f - vec2(0.0,1.0) );
	float vd = dot( gd, f - vec2(1.0,1.0) );

	return vec3( va + u.x*(vb-va) + u.y*(vc-va) + u.x*u.y*(va-vb-vc+vd),   // value
					ga + u.x*(gb-ga) + u.y*(gc-ga) + u.x*u.y*(ga-gb-gc+gd) +  // derivatives
					du * (u.yx*(va-vb-vc+vd) + vec2(vb,vc) - va));
}

vec3 displacement(vec2 coord)
{
	vec3 d = noised(coord * 100);
//	d += noised(coord * 20) * 0.5;
//	d += noised(coord * 40) * 0.25;
//	d += noised(coord * 500) * 0.01;
//	d *= 20.5;
//	d = max(d, -0.8);
	return d;
}

void main() 
{
	vec4 pos = interpolate(tcPos[0], tcPos[1], tcPos[2], tcPos[3]);
	vec2 texCoord = interpolate(tcTexCoord[0], tcTexCoord[1], tcTexCoord[2], tcTexCoord[3]);
	vec3 terrain = displacement(texCoord);
	o.normal = normalize( vec3(-terrain.y, 1.0, -terrain.z) );
	pos.y = terrain.x;

	gl_Position = camera * pos;
	o.instanceID = instanceID;
}