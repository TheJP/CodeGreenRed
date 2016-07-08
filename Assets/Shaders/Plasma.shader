Shader "Unlit/Plasma"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				float v = sin(10 * (i.uv[0] * sin(_Time / 2.0) + i.uv[1] * cos(_Time / 3.0)) + _Time[1]);  

				float cx = i.uv[0] + .5 * sin(_Time[0]);
				float cy = i.uv[1] + .5 * cos(_Time[0]);

				v += sin(sqrt(100 * (cx * cx + cy * cy) + 1 + _Time[0]));

				col.r = sin(v * 3.1418) / (2 * (2.0 + sin(_Time[3])) /5);
				col.g = cos(v * 3.1418) / (2 * (2.0 + sin(_Time[3])) /5);
				col *= 0.3;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
