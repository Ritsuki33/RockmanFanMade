Shader "Custom/ColorChangeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _TargetColor("TragetColor",Color)=(1,1,1,1)
        _Threshold("threshold", Range(0,1))=0.1
        _ChangeColor("ChangeColor",Color)=(1,1,1,1)
        [Space]
        _TargetColor2("TragetColor2",Color)=(1,1,1,1)
        _Threshold2("threshold2", Range(0,1))=0.1
        _ChangeColor2("ChangeColor2",Color)=(1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent"  "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha

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

            uniform float4 _TargetColor;
            uniform float4 _ChangeColor;
            uniform float _Threshold;

            uniform float4 _TargetColor2;
            uniform float4 _ChangeColor2;
            uniform float _Threshold2;
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
                float4 newCol = col;

                float3 diff=col.rbg-_TargetColor.rbg;
                float3 diff2=col.rbg-_TargetColor2.rbg;
                if(dot(diff, diff)/3<=_Threshold){
                    newCol.rgb=_ChangeColor.rbg;
                }
                else if(dot(diff2, diff2)/3<=_Threshold2){
                    newCol.rgb=_ChangeColor2.rbg;
                }
                
                return newCol;
            }
            ENDCG
        }
    }
}
