Shader "Custom/CBOutlineShader"
{
    Properties
    {
        _Color ("color", Color) = (0,0,0,1)
		_Thickness ("thickness", Range(0.0, 1.0)) = 0.1
    }

	CGINCLUDE
	#include "UnityCG.cginc"

	ENDCG

    SubShader
    {
        Tags 
		{ "Queue"="Transparent"
			   "IgnoreProjector" = "True"
		}
        
		Cull Off

		//	くりぬく部分をマスク
		Pass
		{
			ColorMask 0
			ZTest Off
			ZWrite Off
			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
				ZFail Zero
			}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			float4 vert(float4 vertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			half4 frag() : COLOR
			{
				return half4(0,0,0,1);
			}

			ENDCG
		}

		//	シルエットを描く
        Pass
        {
			ColorMask RGB
			Cull Back
			ZTest LEqual
			ZWrite On

			Stencil
			{
				Ref 1
				Comp NotEqual
				Fail Keep
				Pass Keep
				ZFail Keep
			}

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
			#pragma target 3.5

			struct VertexExpansion
			{
				float3 normal;
			};

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : POSITION;
            };

			uniform StructuredBuffer<VertexExpansion> _VertexExpansionArray;
			uniform float _Thickness;
			uniform float4 _Color;

            v2f vert (appdata v, uint index : SV_VertexID)
            {
                v2f o = (v2f)0;
                
				float3 vnormal = _VertexExpansionArray[index].normal;

				float3 normal = mul(UNITY_MATRIX_IT_MV, vnormal);
				float2 basis = TransformViewToProjection(normalize(normal.xy));

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex.xy += basis * _Thickness;	//	プロジェクション座標でxy方向に拡大(Z方向は無意味なので変更なし)

				return o;
            }

            fixed4 frag () : SV_Target
            {
				return _Color;
            }

            ENDCG
        }

		//	ステンシルクリア
		Pass
		{
			ColorMask 0
			ZWrite Off
			ZTest Off
			Stencil
			{
				Ref 0
				Comp Always
				Pass Replace
			}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			float4 vert(float4 vertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			half4 frag() : COLOR
			{
				return half4(0,0,0,1);
			}

			ENDCG
		}
    }

	Fallback "Diffuse"
}
