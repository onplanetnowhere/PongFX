  Shader "SilVR/Legacy Surface" {
    Properties {
      //_MainTex ("Ignore This", 2D) = "white" {}
      _BumpMap ("Normal (RenderTexture)", 2D) = "bump" {}
      _Cube ("Cubemap", CUBE) = "" {}
	  _TrueNorm("True Normal (1=enabled)", Range(0,1)) = 0
	  _imgRes ("Ripple Divisor", float) = 540
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          //float2 uv_MainTex;
          float2 uv_BumpMap;
          float3 worldRefl;
          INTERNAL_DATA
      };
      //sampler2D _MainTex;
      sampler2D _BumpMap;
      samplerCUBE _Cube;
	  float _imgRes;
	  float _TrueNorm;
      void surf (Input IN, inout SurfaceOutput o) {
		  o.Albedo = fixed4(0, 0, 0, 0);

		  fixed4 col = tex2D(_BumpMap, IN.uv_BumpMap);

		  //float2 uv = i.uv;
		  float q = 1 / _imgRes;

		  float2 cauv = IN.uv_BumpMap + float2(0, q);
		  float2 cbuv = IN.uv_BumpMap + float2(0, -q);
		  float2 ccuv = IN.uv_BumpMap + float2(q, 0);
		  float2 cduv = IN.uv_BumpMap + float2(-q, 0);

		  fixed4 d = tex2D(_BumpMap, IN.uv_BumpMap).x;

		  fixed4 ca = tex2D(_BumpMap, cauv).x;
		  fixed4 cb = tex2D(_BumpMap, cbuv).x;
		  fixed4 cc = tex2D(_BumpMap, ccuv).x;
		  fixed4 cd = tex2D(_BumpMap, cduv).x;

		  float diffY = ca - cb;
		  float diffX = cc - cd;

		  float4 c = float4(.5 + -diffX * .5, .5 + -diffY * .5, 1, .5);

          o.Normal = (1-_TrueNorm)*UnpackNormal(tex2D (_BumpMap, IN.uv_BumpMap)) + _TrueNorm*UnpackNormal(c);
          o.Emission = texCUBE (_Cube, WorldReflectionVector (IN, o.Normal)).rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }