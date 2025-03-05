Shader "Custom/GrayScaleShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Main Tex", 2D) = "white" {}
        _GrayScale ("_GrayScale", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags
        { 
            "Queue" = "Transparent" 
            "IgnoreProjector" = "True" 
            "RenderType" = "Transparent" 
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t 
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f 
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };

            sampler2D _MainTex;

            float _GrayScale;
            fixed4 Grayscale(fixed4 color)
            {
                float gray = dot(color.rgb, float3(0.3, 0.59, 0.11));
                return float4(gray, gray, gray, color.a);
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target 
            {
                fixed4 tex = tex2D(_MainTex, i.texcoord);
                fixed4 grayScale = Grayscale(tex);
                
                return lerp(tex, grayScale, _GrayScale);
            }
    
        ENDCG
        }
    }
}
