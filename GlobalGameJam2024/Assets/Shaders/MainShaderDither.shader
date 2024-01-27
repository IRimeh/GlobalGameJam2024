Shader "Unlit/MasterShaderDither"
{
    Properties
    {
        _BaseColor("Color", Color) = (1,1,1,1)
        _BaseMap("Texture", 2D) = "white" {}
        _BlueNoise("Blue Noise", 2D) = "white" {}
        _FresnelPow("Fresnel Pow", float) = 8
        [HDR]_FresnelColor("Fresnel Col", Color) = (1,1,1,1)
        _DitherMinDist("Dither Min Dist", float) = 750
        _DitherMaxDist("Dither Max Dist", float) = 900
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {

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
                float4 normal       : NORMAL;
                float4 tangent      : TANGENT;
            };

            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR;
                float4 shadowCoord : TEXCOORD1;
                float3 positionWS   : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
            };

            //Colors
            float4 _BaseColor;
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;

            TEXTURE2D(_BlueNoise);
            SAMPLER(sampler_BlueNoise);

            float _FresnelPow;
            float4 _FresnelColor;

            float _DitherMinDist;
            float _DitherMaxDist;

            v2f vert(appdata IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normal.xyz, IN.tangent);

                OUT.positionCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                OUT.shadowCoord = GetShadowCoord(positionInputs);
                OUT.normalWS = normalInputs.normalWS;

                return OUT;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, (i.uv * _BaseMap_ST.xy) + _BaseMap_ST.zw);
                baseMap = baseMap * _BaseColor;

                float4 shadowCoord = TransformWorldToShadowCoord(i.positionWS.xyz);
                Light light = GetMainLight(shadowCoord);
                float NdotL = dot(i.normalWS, light.direction);

                // Frensel
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.positionWS);
                float fresnel = pow(1.0f - dot(viewDir, i.normalWS), _FresnelPow);
                fresnel = step(0.3f, fresnel);
                float rimLight = saturate(fresnel * NdotL);
                baseMap.rgb = lerp(baseMap.rgb, _FresnelColor, rimLight);

                // Shadows
                baseMap.rgb = lerp(baseMap.rgb * 0.5f, baseMap.rgb, light.shadowAttenuation);

                float4 dither = SAMPLE_TEXTURE2D(_BlueNoise, sampler_BlueNoise, i.positionCS * 0.01f);
                float dist = length(i.positionWS);
                float perc = saturate((dist - _DitherMinDist) / _DitherMaxDist);
                //return dither;
                //return float4(perc, perc, perc, 1);
                if (perc > dither.r)
                    discard;
                //baseMap.a = 0;

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

        Pass{
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
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

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 positionOS   : POSITION;
            };

            struct v2f
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD2;
            };

            float _DitherMinDist;

            v2f vert(appdata IN)
            {
                v2f OUT;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;

                return OUT;
            }

            float4 frag(v2f i) : SV_Target
            {
                float dist = length(i.positionWS);
                if (dist > _DitherMinDist)
                    discard;

                return 1;
            }
            ENDHLSL
        }
    }
}