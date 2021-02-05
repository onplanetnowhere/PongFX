Shader "SilVR/True Normal Calculation"
{
	Properties
	{
		_MainTex ("Normal Out (Render Texture)", 2D) = "white" {}
		_imgRes("Image Resolution", float) = 540
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
			float _imgRes;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				//float2 uv = i.uv;
				float q = 1 / _imgRes;

				float2 cauv = i.uv + float2(0, q);
				float2 cbuv = i.uv + float2(0, -q);
				float2 ccuv = i.uv + float2(q, 0);
				float2 cduv = i.uv + float2(-q, 0);

				float2 caauv = i.uv + float2(q, q);
				float2 cbbuv = i.uv + float2(q, -q);
				float2 cccuv = i.uv + float2(q, q);
				float2 cdduv = i.uv + float2(-q, -q);

				fixed4 d = tex2D(_MainTex, i.uv).y;

				fixed4 ca = tex2D(_MainTex, cauv).y;
				fixed4 cb = tex2D(_MainTex, cbuv).y;
				fixed4 cc = tex2D(_MainTex, ccuv).y;
				fixed4 cd = tex2D(_MainTex, cduv).y;

				fixed4 caa = tex2D(_MainTex, cauv).y;
				fixed4 cbb = tex2D(_MainTex, cbuv).y;
				fixed4 ccc = tex2D(_MainTex, ccuv).y;
				fixed4 cdd = tex2D(_MainTex, cduv).y;

				float diffXY1 = caa - cbb;
				float diffXY2 = ccc - cdd;

				float diffY = ca - cb;
				float diffX = cc - cd;

				float4 c = float4(.5 + -diffX*.5, .5 + -diffY*.5, 1, .5);
				//c = normalize(c);

				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return c;
			}
			ENDCG
		}
	}
}
