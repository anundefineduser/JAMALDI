Shader "Legacy Shaders/Lightmapped/Transparent" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _LightMap ("Lightmap", 2D) = "white" {}
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        sampler2D _LightMap;
        fixed4 _Color;

        struct Input {
            float2 uv_MainTex;
            float2 uv_LightMap;
            fixed4 color : COLOR;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            fixed4 lightmap = tex2D(_LightMap, IN.uv_LightMap);

            o.Albedo = c.rgb * IN.color.rgb;
            o.Alpha = c.a * IN.color.a;
            o.Emission = lightmap.rgb;
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Transparent/VertexLit"
}
