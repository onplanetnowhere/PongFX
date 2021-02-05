  Shader "SilVR/Water Surface" {
    Properties {
      _BumpMap ("Render Plane (RenderTexture)", 2D) = "bump" {}
      _Cube ("Cubemap", CUBE) = "" {}
	  _TrueNorm("True Normal (1=enabled)", Range(0,1)) = 0
	  _imgRes("Ripple Divisor", float) = 540
	  _Alpha("Alpha", Range(0,1)) = 1
	  _AlphaMask("Transparency Mask", 2D) = "White" {}
	  _AlphaWeight("Alpha Mask Weight", Range(0,1)) = 1
    }
    SubShader {
      Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
      CGPROGRAM
      #pragma surface surf Lambert alpha
      struct Input {
          //float2 uv_MainTex;
          float2 uv_BumpMap;
          float3 worldRefl;
          INTERNAL_DATA
      };
      //sampler2D _MainTex;
      sampler2D _BumpMap;
	  sampler2D _AlphaMask;
      samplerCUBE _Cube;
	  float _imgRes;
	  float _TrueNorm;
	  float _Alpha;
	  float _AlphaWeight;

	  //float _Aspect;
      void surf (Input IN, inout SurfaceOutput o) {
		  o.Albedo = fixed4(0, 0, 0, 0);

		  fixed4 col = tex2D(_BumpMap, IN.uv_BumpMap);

		  //float2 uv = i.uv;
		  float q = 1 / _imgRes;

		  float2 uv = float2(IN.uv_BumpMap.x*.5+.5, IN.uv_BumpMap.y);

		  float2 cauv = uv + float2(0, q);
		  float2 cbuv = uv + float2(0, -q);
		  float2 ccuv = uv + float2(q*.5, 0);
		  float2 cduv = uv + float2(-q*.5, 0);

		  fixed4 d = tex2D(_BumpMap, uv).x;

		  fixed4 ca = tex2D(_BumpMap, cauv).x;
		  fixed4 cb = tex2D(_BumpMap, cbuv).x;
		  fixed4 cc = tex2D(_BumpMap, ccuv).x;
		  fixed4 cd = tex2D(_BumpMap, cduv).x;

		  float diffY = ca - cb;
		  float diffX = cc - cd;

		  //float4 c = float4(.5 + -diffX * .5, .5 + -diffY * .5, 1, .5);
		  float4 c = float4(.5 * 0 + -diffX * 2.5 * 2, .5 * 0 + -diffY * 2.5 * 2, 1 , 1);

          //o.Normal = (1-_TrueNorm)*UnpackNormal(tex2D (_BumpMap, uv)) + _TrueNorm*UnpackNormal(c);
		  //o.Normal = UnpackNormal((1-_TrueNorm) * tex2D (_BumpMap, uv) + _TrueNorm * c);
		  o.Normal = ((1 - _TrueNorm) * tex2D(_BumpMap, uv) + _TrueNorm * c) * fixed3(0.5, -0.5, 0.0);
		  //o.Normal = tex2D(_BumpMap, uv) - c;

		  o.Alpha = _Alpha*(1-_AlphaWeight) + tex2D(_AlphaMask, IN.uv_BumpMap).x*_AlphaWeight*_Alpha;
		  o.Emission = texCUBE (_Cube, WorldReflectionVector (IN, o.Normal)).rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }