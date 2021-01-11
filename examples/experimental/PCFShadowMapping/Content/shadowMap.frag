#version 430 core

uniform sampler2D texShadowMap;
uniform vec3 lightPosition;
uniform vec3 ambient;

in blockData
{
	vec4 position_LS;
	vec3 position;
	vec3 normal;
} i;

float pcf2Smooth(vec3 coord)
{
	vec2 coord_ts = coord.xy * 0.5 + 0.5;

	vec4 q = step(0.0, textureGather(texShadowMap, coord_ts, 0) - coord.z);

	ivec2 texSize = textureSize(texShadowMap, 0);
	//weights
	vec2 weights = fract(coord_ts * texSize - 0.5);

	//bilinear interpolation
	vec2 x = mix(q.xw, q.yz, weights.x);
	return mix(x[1], x[0], weights.y);
}

float shadowLookup(ivec2 coord, float depth)
{
	return step(0.0, texelFetch(texShadowMap, coord, 0).r - depth);
}

float pcf(vec3 coord)
{
	ivec2 iCoord_ts = ivec2((coord.xy * 0.5 + 0.5) * textureSize(texShadowMap, 0));
	float result = 0.0;
	int kernelSizeHalf = 3;
	float weightSum = 0;
	for(int x = -kernelSizeHalf; x <= kernelSizeHalf; ++x)
	{
		for(int y = -kernelSizeHalf; y <= kernelSizeHalf; ++y)
		{
			ivec2 newCoord = iCoord_ts + ivec2(x,y);
			weightSum ++;
			result += shadowLookup(newCoord, coord.z);
		}
	}
	return result / weightSum;
}

// exponential smooth min (k = 32);
float smin( float a, float b, float k )
{
	float res = exp2( -k * a ) + exp2( -k * b );
	return -log2( res ) / k;
}

// polynomial smooth min (k = 0.1);
float sminCubic( float a, float b, float k )
{
    float h = max( k-abs(a-b), 0.0 )/k;
    return min( a, b ) - h*h*h*k*(1.0/6.0);
}

float pcfDistField(vec3 coord)
{
	vec2 coord_ts = (coord.xy * 0.5 + 0.5) * textureSize(texShadowMap, 0);
	ivec2 iCoord_ts = ivec2(coord_ts - fract(coord_ts));
	float minDist = 1e10;
	int kernelSizeHalf = 20;
	for(int x = -kernelSizeHalf; x <= kernelSizeHalf; ++x)
	{
		for(int y = -kernelSizeHalf; y <= kernelSizeHalf; ++y)
		{
			ivec2 newCoord = iCoord_ts + ivec2(x, y);
			float lit = shadowLookup(newCoord, coord.z);
			if(lit < 0.5)
			{
				float dist = distance(coord_ts, newCoord);
				minDist = smin(dist, minDist, 3);

			}
		}
	}
	return smoothstep(0.7, float(kernelSizeHalf), minDist);
}

float pcfQuartil(vec2 from, vec2 to, float depth)
{
	float result = 0.0;
	int weightSum = 0;
	for(float x = (from.x); x <= (to.x); ++x)
	{
		for(float y = (from.y); y <= (to.y); ++y)
		{
			ivec2 newCoord = ivec2(x,y);
			weightSum++;
			result += shadowLookup(newCoord, depth);
		}
	}
	return result / weightSum;
}

float pcf3Smooth(vec3 coord)
{
	int kernelSizeHalf = 1;
	vec2 coord_ts = (coord.xy * 0.5 + 0.5) * textureSize(texShadowMap, 0);
	vec4 q;
	q[0] = pcfQuartil(coord_ts, coord_ts + kernelSizeHalf, coord.z);
	q[1] = pcfQuartil(vec2(coord_ts.x - kernelSizeHalf, coord_ts.y), vec2(coord_ts.x, coord_ts.y + kernelSizeHalf), coord.z);
	q[2] = pcfQuartil(coord_ts - kernelSizeHalf, coord_ts, coord.z);
	q[3] = pcfQuartil(vec2(coord_ts.x, coord_ts.y - kernelSizeHalf), vec2(coord_ts.x + kernelSizeHalf, coord_ts.y), coord.z);

//	return ( q[0] + q[1] + q[2] + q[3] ) / 4.0;
	//bilinear interpolation weights
	vec2 weights = fract(coord_ts);
	//bilinear interpolation
	vec2 x = mix(q.yz, q.xw, weights.x);
	return mix(x[1], x[0], weights.y);
}

float lambert(vec3 n, vec3 l)
{
	return max(0, dot(n, l));
}

out vec4 color;

void main() 
{
	color = vec4(i.normal, 1);

	vec3 coord = i.position_LS.xyz / i.position_LS.w;

	vec3 lighting = ambient;

	vec3 toLight = normalize(lightPosition - i.position);
	lighting += pcf3Smooth(coord) * lambert(normalize(i.normal), toLight) * vec3(1);

	color = vec4(lighting, 1);
}