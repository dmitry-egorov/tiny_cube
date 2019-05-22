Shader "Custom/Character"
{
	Properties
	{
	  _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	  _OccludedColor("Occluded Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }

		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			half4 _Color;

			struct appdata
			{
				float4 position : POSITION;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.position);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				return _Color;
			}
			ENDCG
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Greater

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			half4 _OccludedColor;

			struct appdata
			{
				float4 position : POSITION;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.position);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				float t = fmod(i.position.x + i.position.y, 2);
				return lerp(_OccludedColor, fixed4(0, 0, 0, 0), t);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}