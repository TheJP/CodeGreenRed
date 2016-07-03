Shader "Unlit/GreenShade"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SineVal ("Sine", Range(0.0, 1.0)) = 0.0
		_GreenVal ("Green", Range(0.0, 1.0)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
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
				float3 normal : NORMAL;
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

			float _SineVal;
			float _GreenVal;
			float blu = 1.0;
			
			v2f vert (appdata v)
			{
				v2f o;
				//float3 bla = (_SineVal, 1.0, 1.0);
				float3  fVector = { _SineVal, _SineVal*0.3f, _SineVal*0.4f };

				float4 newpos =  v.vertex;
				newpos.z += sin(5.0*newpos.x + _Time[1]) * 0.05;
				newpos.x += sin(5.0*newpos.x + _Time[2]) * 0.05;

				o.vertex = mul(UNITY_MATRIX_MVP, newpos);
				o.uv = UnityObjectToWorldNormal(v.normal);
				//v.vertex += noise(o.vertex)*100;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				col.r = 1.0;// + i.uv * 0.5;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
