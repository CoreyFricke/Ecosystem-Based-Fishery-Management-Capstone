Shader "Custom/Terrain"
{
    Properties
    {

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D terrainGradiant;
        float minTerrainHeight;
        float maxTerrainHeight;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float 3 worldPosY = IN.worldPos.y;

            float heightValue = saturate((worldPosY - minTerrainHeight) / (maxTerrainHeight - minTerrainHeight));

            o.Albedo = tex2D(terrainGradiant, float2(0, heightValue));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
