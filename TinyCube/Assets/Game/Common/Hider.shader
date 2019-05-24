Shader "Custom/Hider"
{
	Properties
	{
	}
	SubShader
	{
        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
        }
        
        Blend Zero One
        ZWrite Off
        ZTest Off
            
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

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
				return 0;
			}
			ENDCG
		}

		
	}
}