// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Stencil/MaxZOneLess"
{
    Properties{
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry-1"}
        //No Color
        ColorMask 0
        ZWrite off
        
        Stencil{
            Ref 1
            Comp always
            Pass replace
        }
        
        Pass{
            Cull Back
            ZTest Less

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            float4 _Color;
            struct appdata
            {
                float4 vertex : POSITION;
            };
    
            struct v2f
            {
                float4 pos : SV_POSITION;
            };
            
            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            half4 frag(v2f i) : COLOR
            {
                //Sets color
                return _Color;
            }
    
            ENDCG
        }
    }
}
