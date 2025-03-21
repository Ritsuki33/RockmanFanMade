﻿Shader "Sprites/RockmanShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        // キャラクターのカラー色変更
        _TargetColor("TragetColor",Color)=(1,1,1,1)
        _Threshold("threshold", Range(0,1))=0.1
        _ChangeColor("ChangeColor",Color)=(1,1,1,1)
        [Space]
        _TargetColor2("TragetColor2",Color)=(1,1,1,1)
        _Threshold2("threshold2", Range(0,1))=0.1
        _ChangeColor2("ChangeColor2",Color)=(1,1,1,1)

              // キャラクターのエッジの発行
        [Space] 
         _RimLightColor("RimLightColor",Color) = (1,1,1,1)
        _FadeLight("FadeLight",Range(0,1)) = 0

        // キャラクターの発行（全体）
        [Space]
        _FadeColor("FadeColor",Color)=(1,1,1,1)
        _IsFadeColor("IsFadeColor",Range(0,1)) = 0
        _BrightThreshold("BrightThreshold",Range(0,1))=0.2
        _DarkBrightPower("DarkBrightPower",Range(0,1))=0.25
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
            float2 _MainTex_TexelSize;

            uniform float4 _TargetColor;
            uniform float4 _ChangeColor;
            uniform float _Threshold;

            uniform float4 _TargetColor2;
            uniform float4 _ChangeColor2;
            uniform float _Threshold2;

            uniform float4 _FadeColor;
            uniform float _IsFadeColor;
            uniform float _BrightThreshold;
            uniform float _DarkBrightPower;

            uniform float4 _RimLightColor;
            float _FadeLight;

            // キャラクターのカラー変更
            fixed4 changeColor (fixed4 col){
                float4 newCol = col;

                float3 diff=col.rbg-_TargetColor.rbg;
                float3 diff2=col.rbg-_TargetColor2.rbg;
                if(dot(diff, diff)/3<=_Threshold){
                    newCol.rbg=_ChangeColor.rbg;
                }
                else if(dot(diff2, diff2)/3<=_Threshold2){
                    newCol.rbg=_ChangeColor2.rbg;
                }
                
                return newCol;
            }

            // エッジのライティング
            fixed4 lightingEdge(fixed4 col,float2 uv)
            {
                fixed4 newCol = col;
                fixed2 offset[4] = {fixed2(0, 1), fixed2(-1,  0),fixed2(1,  0),fixed2(0,  -1)};
                fixed3 changeCol = lerp(col.rgb,_RimLightColor.rgb,_FadeLight);
                if(col.a != 0){
                    for(int j = 0; j < 4; j++){
                        fixed2 sample_uv=uv + offset[j] * _MainTex_TexelSize.xy;
                        if(sample_uv.x<0||1<sample_uv.x||sample_uv.y<0||1<sample_uv.y){
                            newCol = fixed4(changeCol, 1);
                            break;
                        }
                        else{
                            fixed4 check = tex2D(_MainTex, uv + offset[j] * _MainTex_TexelSize.xy);

                            if(check.a == 0.0f){
                                newCol = fixed4(changeCol, 1);
                                break;
                            }
                        }
                    }
                }
                return newCol;
            }

            // フラッシュ
            fixed4 Flash(fixed4 col)
            {
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
                
                fixed4 newCol = changeColor(col);
                
                newCol = lightingEdge(newCol,i.uv);
                newCol = Flash(newCol);

               

                return newCol;
            }

           
            ENDCG
        }
    }
}
