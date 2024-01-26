Shader "Unlit/MasterShader"
{
    Properties
    {
        _BaseColor("Color", Color) = (1,1,1,1)
        _BaseMap("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 100
        ZWrite On

        Pass
        {
            Stencil
            {
                Ref[_Stencil]
                Pass Replace
                Comp Always
            }

            Tags 
            { 
                "LightMode" = "UniversalForward" 
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR;
            };

            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR;
                float4 shadowCoord : TEXCOORD1;
                float3 positionWS   : TEXCOORD2;
            };

            //Colors
            float4 _BaseColor;
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;

            v2f vert(appdata IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                OUT.shadowCoord = GetShadowCoord(positionInputs);

                return OUT;
            }

            float4 frag(v2f IN) : SV_Target
            {
                float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, (IN.uv * _BaseMap_ST.xy) + _BaseMap_ST.zw);
                baseMap = baseMap * _BaseColor;

                float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS.xyz);
                Light light = GetMainLight(shadowCoord);

                // Shadows
                baseMap.rgb = lerp(baseMap.rgb * 0.2f, baseMap.rgb, light.shadowAttenuation);

                return baseMap;
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            Cull Back

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float _Cutoff;
            CBUFFER_END

            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x gles
            //#pragma target 4.5

            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // GPU Instancing 
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            //#pragma vertex vert
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"

            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/DepthOnly"
    }
}