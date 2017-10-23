// Created by Michael Marshall, 23 Oct 2017

Shader "Unlit/PhasingShader"
{
	SubShader
	{
		Pass
		{
			Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Transform vertex in world coordinates to camera coordinates
				o.vertex = UnityObjectToClipPos(v.vertex);

				// Change the transparency of the vertex based on time, and make it purple coloured
				float alphaVal = 128.0f + sin(_Time.y*6) * 64.0f;
				o.color = float4(colourRGBA.r/255.0f, colourRGBA.g/255.0f, colourRGBA.b/255.0f, colourRGBA.a/255.0f);
				
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				return v.color;
			}
			ENDCG
		}
	}
}
