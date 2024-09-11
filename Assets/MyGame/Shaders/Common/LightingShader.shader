Shader "Custom/LightingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightColor("LightColor",Color) = (1,1,1,1)

        _FadeLight("FadeLight",Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull off

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float4 _LightColor;
            float2 _MainTex_TexelSize;
            float _FadeLight;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 offset[4] = {fixed2(0, 1), fixed2(-1,  0),fixed2(1,  0),fixed2(0,  -1)};

                fixed4 col = tex2D(_MainTex, i.uv);
                fixed3 newCol = lerp(col.rgb,_LightColor.rgb,_FadeLight);
                if(col.a != 0){
                    for(int j = 0; j < 4; j++){
                        fixed2 sample_uv=i.uv + offset[j] * _MainTex_TexelSize.xy;
                        if(sample_uv.x<0||1<sample_uv.x||sample_uv.y<0||1<sample_uv.y){
                            col = fixed4(newCol, 1);
                            break;
                        }
                        else{
                             fixed4 check = tex2D(_MainTex, i.uv + offset[j] * _MainTex_TexelSize.xy);

                            if(check.a == 0.0f){
                                col = fixed4(newCol, 1);
                                break;
                            }
                        }
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}
