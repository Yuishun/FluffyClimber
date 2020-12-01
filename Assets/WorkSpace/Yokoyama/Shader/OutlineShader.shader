// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Color", color) = (1,1,1,1)
		_OutlineThreshold ("Outline Threshold", Range(0, 1)) = 0.25
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma geometry geom
            #pragma fragment frag
            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 viewPos : TEXCOORD1;
				float4 normal : TEXCOORD2;
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _OutlineColor;
			float _OutlineThreshold;

            appdata vert (appdata v)
            {
				appdata ret = v;
				return ret;
            }

			[maxvertexcount(3)]
			void geom(triangle appdata input[3], inout TriangleStream<v2f> outStream)
			{
				/*v2f o;

				float3 vecA = input[1].vertex - input[0].vertex;
				float3 vecB = input[2].vertex - input[0].vertex;
				float3 normal = cross(vecA, vecB);

				normal = mul(UNITY_MATRIX_MV, normal).xyz;

				float result = dot(normalize(normal), float3(0, 0, 1));
				o.color = (result < 0.25f) ? _OutlineColor : float4(0, 0, 0, 1);

				for (int i = 0; i < 3; ++i)
				{
					o.pos = UnityObjectToClipPos(input[i].vertex);
					o.uv = input[i].uv;
					outStream.Append(o);
				}

				outStream.RestartStrip();*/

				v2f o;

				for (int i = 0; i < 3; ++i)
				{
					o.pos = UnityObjectToClipPos(input[i].vertex);
					o.uv = input[i].uv;
					o.viewPos = mul(UNITY_MATRIX_MV, input[i].vertex);
					o.normal = mul(UNITY_MATRIX_MV, normalize(input[i].normal));
					outStream.Append(o);
				}

				outStream.RestartStrip();

			}

            fixed4 frag (v2f i) : SV_Target
            {
				/*return i.color;*/
				float viewCos = dot(normalize(i.viewPos * -1), normalize(i.normal));
				return (viewCos > _OutlineThreshold) ? fixed4(0, 0, 0, 1) : _OutlineColor;
            }
            ENDCG
        }
    }
}
