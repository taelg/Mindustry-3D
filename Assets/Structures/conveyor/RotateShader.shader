Shader "aaa_custom/Rotate"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Dst Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Opp("Operation", Float) = 0

        _MainTex ("Texture", 2D) = "white" {}
        _TopTex ("Top Texture", 2D) = "white" {}
        _Rotation("Rotation (degrees)", float) = 0
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        LOD 100

        // First pass: draw the rotated bottom texture
        Pass
        {
            Blend One Zero
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Rotation;

            float2 RotateMat(float2 uv) {
                float2 newUV;
                newUV = uv * 2 - 1;

                float radians = _Rotation * 0.0174533; // Conversion factor from degrees to radians

                float c = cos(radians);
                float s = sin(radians);

                float2x2 mat = float2x2(c, -s,
                                        s, c);
                newUV = mul(mat, newUV);

                newUV = newUV * 0.5 + 0.5;

                return newUV;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                o.uv.xy = RotateMat(o.uv.xy);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv.xy);
                return col;
            }
            ENDCG
        }

        // Second pass: draw the top texture with transparency
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag2
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _TopTex;
            float4 _TopTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _TopTex);
                return o;
            }

            fixed4 frag2 (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_TopTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
