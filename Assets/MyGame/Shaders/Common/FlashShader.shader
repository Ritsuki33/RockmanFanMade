Shader "Custom/FlashShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FadeColor("FadeColor",Color)=(1,1,1,1)
        _IsFadeColor("IsFadeColor",Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent"  "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float4 _FadeColor;
            uniform float _IsFadeColor;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float bright= 0.2126 * col.r + 0.7152 * col.b + 0.0722 * col.g;
                float4 newCol=col;
                if(bright>0.2){
                    newCol = lerp(col, float4(_FadeColor.rgb,col.a), _IsFadeColor);
                }
                else{
                 newCol = lerp(col, float4(_FadeColor.rgb,col.a), _IsFadeColor/4);
                }
                return newCol;
            }
            ENDCG
        }
    }
}
