﻿// Original Cg/HLSL code stub copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Adapted for COMP30019 by Jeremy Nicholson, 10 Sep 2012
// Adapted further by Chris Ewin, 23 Sep 2013
// Adapted further (again) by Alex Zable (port to Unity), 19 Aug 2016
// Adapted further (again, again) by Michael Marshall, 23 Oct 2017

//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/PhongShader"
{
	Properties
	{
		_PointLightColor ("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition ("Point Light Position", Vector) = (0.0, 0.0, 0.0)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Convert Vertex position and corresponding normal into world coords.
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

				// Transform vertex in world coordinates to camera coordinates
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				// Change the colour of the vertex based on height (appears rainbow coloured repeating):
				float4 colourRGBA;
				// Red
				if(abs(worldVertex.y % 7) <= 1) {
					colourRGBA = float4(255.0f, 0.0f, 0.0f, 0.0f);
				}
				// Orange
				else if(abs(worldVertex.y % 7) <= 2) {
					colourRGBA = float4(255.0f, 127.0f, 0.0f, 0.0f);
				}
				// Yellow
				else if(abs(worldVertex.y % 7) <= 3) {
					colourRGBA = float4(255.0f, 255.0f, 0.0f, 0.0f);
				}
				// Green
				else if(abs(worldVertex.y % 7) <= 4) {
					colourRGBA = float4(0.0f, 255.0f, 0.0f, 0.0f);
				}
				// Blue
				else if(abs(worldVertex.y % 7) <= 5) {
					colourRGBA = float4(0.0f, 0.0f, 255.0f, 0.0f);
				}
				// Indigo
				else if(abs(worldVertex.y % 7) <= 6) {
					colourRGBA = float4(75.0f, 0.0f, 130.0f, 0.0f);
				}
				// Violet
				else {
					colourRGBA = float4(139.0f, 0.0f, 255.0f, 0.0f);
				}

				o.color = float4(colourRGBA.r/255.0f, colourRGBA.g/255.0f, colourRGBA.b/255.0f, colourRGBA.a/255.0f);

				// Pass out the world vertex position and world normal to be interpolated
				// in the fragment shader (and utilised)
				o.worldNormal = worldNormal;
				o.worldVertex = worldVertex;
				
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				vertOut o;

				// Our interpolated normal might not be of length 1
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = 1;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 1;
				float Kd = 1;
				float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
				float LdotN = dot(L, interpNormal);
				float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);
				
				// Calculate specular reflections
				float Ks = 1;
				float specN = 5; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);

				// Using classic reflection calculation:
				//float3 R = normalize((2.0 * LdotN * interpNormal) - L);
				//float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(V, R)), specN);

				// Using Blinn-Phong approximation:
				specN = 25; // We usually need a higher specular power when using Blinn-Phong
				float3 H = normalize(V + L);
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

				// Combine Phong illumination model components
				o.color = float4(0.0f, 0.0f, 0.0f, 0.0f);
				o.color.rgb = amb.rgb + dif.rgb + spe.rgb;
				o.color.a = v.color.a;

				return o.color;
			}
			ENDCG
		}
	}
}
