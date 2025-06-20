Shader "Unlit/ProgressFill"
{
     Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed t = i.uv.x;
                fixed4 c;
                if (t < 0.25) {
                    c = lerp(fixed4(0.98,0.29,0.29,1), fixed4(1.0,0.66,0.47,1), t/0.25);
                } else if (t < 0.5) {
                    c = lerp(fixed4(1.0,0.66,0.47,1), fixed4(1.0,0.76,0.39,1), (t-0.25)/0.25);
                } else if (t < 0.75) {
                    c = lerp(fixed4(1.0,0.76,0.39,1), fixed4(1.0,1.0,0.36,1), (t-0.5)/0.25);
                } else {
                    c = lerp(fixed4(1.0,1.0,0.36,1), fixed4(0.75,1.0,0.2,1), (t-0.75)/0.25);
                }
                return tex2D(_MainTex, i.uv) * c;
            }
            ENDCG
        }
    }
}
