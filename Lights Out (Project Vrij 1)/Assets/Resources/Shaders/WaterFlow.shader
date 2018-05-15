Shader "ProjVrij/WaterFlow"
{
	Properties
	{
		_WaveScale ("Wave Scale", Range(0.02, 0.20)) = 0.08
		_Color ("Color (RGB)", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Albedo Texture", 2D) = "white" {}
		[NoScaleOffset] _BumpMap ("Normal Texture", 2D) = "bump" {}
		_WaveSpeed ("Wave Speed", Vector) = (20, 10, -15, -5)
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	uniform float4 _WaveSpeed;
	uniform float4 _WaveOffset;
	uniform float _WaveScale;

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 bumpuv[2] : TEXCOORD0;
		float3 viewDir : TEXCOORD2;
		UNITY_FOG_COORDS(3)
	};

	v2f vert(appdata v)
	{
		v2f o;
		float4 s;

		o.pos = UnityObjectToClipPos(v.vertex);

		float4 temp;
		float4 wpos = mul(unity_ObjectToWorld, v.vertex);
		temp.xyzw = wpos.xyzw * _WaveScale * _WaveOffset;
		o.bumpuv[0] = temp.xy * float2(.4, .45);
		o.bumpuv[1] = temp.wz;

		o.viewDir.xzy = normalize( WorldSpaceViewDir(v.vertex) );
		UNITY_TRANSFER_FOG(o,o.pos);

		return o;
	}

	ENDCG

	SubShader {
		Tags{ "RenderType" = "Opaque" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			sampler2D _BumpMap;
			sampler2D _MainTex;

			half4 frag(v2f i) : COLOR
			{
				half3 bump1 = UnpackNormal(tex2D( _BumpMap, i.bumpuv[0])).rgb;
				half3 bump2 = UnpackNormal(tex2D( _BumpMap, i.bumpuv[1])).rgb;
				half3 bump = (bump1 + bump2) * 0.5;

				half fresnel = dot(i.viewDir, bump);
				half4 water = tex2D(_MainTex, half2(fresnel, fresnel));

				half4 mainTex = tex2D(_MainTex, i.bumpuv[0]);

				half4 col;
				col.rgb = lerp(water.rgb, mainTex.rgb, water.a);
				col.a = mainTex.a;

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}