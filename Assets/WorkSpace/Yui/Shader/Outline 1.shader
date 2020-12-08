Shader "Custom/Outline" {
	Properties{
		_MainTex("MainTex", 2D) = ""{}
		Threshold_L("Low_Threshold",Float)=0.1
		Threshold_H("High_Threshold",Float) = 0.9
		_OutLineColor("LineColor",Color)=(0,0,0,0)
	}

		SubShader{
			Pass {
				CGPROGRAM

				#include "UnityCG.cginc"

				#pragma vertex vert_img
				#pragma fragment frag

				sampler2D _MainTex;
				fixed Threshold_L;
				fixed Threshold_H;
				fixed4 _OutLineColor;

				fixed4 frag(v2f_img i) : COLOR {
					fixed4 c = tex2D(_MainTex, i.uv);
				if (i.uv.x < Threshold_L || i.uv.x > Threshold_H
					|| i.uv.y < Threshold_L || i.uv.y > Threshold_H)
					c = _OutLineColor;
					return c;
				}

				ENDCG
			}
	}
}