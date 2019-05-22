Shader "Custom/Solid"
{
	Properties
	{
	  _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
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

		
	}
	FallBack "Diffuse"
}