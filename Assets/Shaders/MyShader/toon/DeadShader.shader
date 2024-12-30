Shader "Shader Forge/DeadShader" {
    Properties {
        [Header(Texture)]
            _MainTex ("MainTex", 2D) = "white" {}
            _LightMap ("LightMap", 2D) = "white" {}
            _MainColor("Main Color", Color) = (1,1,1)
	        _ShadowColor ("Shadow Color", Color) = (0.7, 0.7, 0.8)
	        _ShadowRange ("Shadow Range", Range(0, 1)) = 0.5
            _ShadowSmooth("Shadow Smooth", Range(0, 1)) = 0.02
      [HDR] _RimColor ("Rim Color", Color) = (0.8, 0.8, 0.8,1.0)
            _RimMin("RimMin", Range(0, 1)) = 0.845
            _RimMax("Rimax", Range(0, 1)) = 1
            _RimSmooth("RimSmooth", Range(0, 5)) = 2.74
            _SpecPow("高光次幂",Range(1,90)) = 3
      [HDR] _MySpecColor("MySpecColor", Color) = (0.5,0.5,0.5)
        [Toggle] _LIGHTENABLE("LIGHTENABLE",Float) = 1  
            _MyTime("MyTime", Range(0, 1)) = 0
            _DissolveMap ("DissolveMap", 2D) = "white" {}
      [HDR] _DissolveColor("DissolveColor", Color) = (18.0,0,0)
        //开关是否开启多光源效果
        [Toggle(_AdditionalLights)] _AddLights ("AddLights", Float) = 1
    }
    SubShader {
          Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="UniversalForward"
            }
            Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #pragma target 3.0
            #pragma multi_compile  _MAIN_LIGHT_SHADOWS
			#pragma multi_compile  _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile  _SHADOWS_SOFT
            #pragma shader_feature_local _LIGHTENABLE_ON
            #pragma multi_compile_fog
            #pragma shader_feature _AdditionalLights
            
            sampler2D _MainTex;
            sampler2D _LightMap;
	        float4 _MainTex_ST;
            half3 _MainColor;
	        half3 _ShadowColor;
            half _ShadowRange;
            half _ShadowSmooth;
            half4 _RimColor;
            half _RimMin;
            half _RimMax;
            half _RimSmooth;
            half _SpecPow;
            half3 _MySpecColor;
            sampler2D _DissolveMap;
            half _MyTime;
            half3 _DissolveColor;
            

            struct VertexInput {
                float4 vertex : POSITION;
                float4 color : COLOR0;
                float4 normal : NORMAL;
                float2 uv :TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 color : COLOR0;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
		        float3 worldPos : TEXCOORD2;
                float fogCoord : TEXCOORD3;
            };



            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normal.xyz);
                o.worldNormal = vertexNormalInput.normalWS;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.pos = vertexInput.positionCS;
                o.worldPos = vertexInput.positionWS;
                o.color = v.color;
                o.fogCoord = ComputeFogFactor(o.pos.z);
                return o;
            }


            float4 frag(VertexOutput i) : COLOR {
                half4 col = 1;
                half4 mainTex = tex2D(_MainTex, i.uv);
           
                half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
                half3 worldNormal = normalize(i.worldNormal);

                // 阴影光照数据
                // #pragma multi_compile  _MAIN_LIGHT_SHADOWS
			    // #pragma multi_compile  _MAIN_LIGHT_SHADOWS_CASCADE
			    // #pragma multi_compile  _SHADOWS_SOFT
                 float4 SHADOW_COORDS = TransformWorldToShadowCoord(i.worldPos);
				 Light mainLight = GetMainLight(SHADOW_COORDS);
                // shadow 值 mainLight.shadowAttenuation
                // half ramp = smoothstep(0, _ShadowSmooth, (halfLambert  - _ShadowRange) * LightMapColor.g) * mainLight.shadowAttenuation;
                //Light mainLight = GetMainLight();
               half3 worldLightDir = mainLight.direction;

                //边缘光
                half f =  1.0 - saturate(dot(viewDir, worldNormal));
                half rim = smoothstep(_RimMin, _RimMax, f);
                rim = smoothstep(0, _RimSmooth, rim);
                half3 rimColor = rim * _RimColor.rgb;

                #if defined _LIGHTENABLE_ON
                half4 LightMapColor = tex2D(_LightMap, i.uv);
                //新高光
                half3 reflectDir = normalize(reflect(-worldLightDir, worldNormal));
                float spec =  pow(max(0.0,dot(reflectDir, viewDir)), _SpecPow);
                spec = step(0.5f - LightMapColor.b, spec);
                half3 specular =  _MySpecColor *spec *  LightMapColor.r;
                #else
                half4 LightMapColor = half4(1,1,0,1);
                half3 specular = half3(0,0,0);
                #endif

                float halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
                half ramp = smoothstep(0, _ShadowSmooth, (halfLambert  - _ShadowRange) * LightMapColor.g)* mainLight.shadowAttenuation;
                half3 finalLight = mainLight.color;
                //其它光源计算
                #ifdef _AdditionalLights
                    int pixelLightCount = GetAdditionalLightsCount();            //获取副光源个数，是整数类型

                    for(int index = 0; index < pixelLightCount; index++)
                    {
                        Light light = GetAdditionalLight(index, i.worldPos);     //获取其它的副光源世界位置
                        float addLambert = dot(worldNormal, light.direction); // 不需要 HalfLambert 点光源
                        ramp += smoothstep(0, _ShadowSmooth, (addLambert  - _ShadowRange) * LightMapColor.g)* light.shadowAttenuation * light.distanceAttenuation;
                        // to do 暂时先忽略边缘光和高光吧。。。
                        // 灯光叠加，目前这种叠加方式可能会导致太亮
                        finalLight += light.color * min(1,light.distanceAttenuation);
					}
                #endif

                half3 ambient = half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w); 
                finalLight += ambient;
                half3 diffuse = lerp(_ShadowColor * _MainColor, _MainColor, ramp);
                diffuse *= mainTex.xyz;
                col.rgb = (diffuse.rgb + specular + rimColor) * finalLight;

                col *= 1.0 - (max(0.5,min(1.0,_MyTime*4.0)) - 0.5) * 2.0;

                 half4 dissolveTex = tex2D(_DissolveMap,i.uv);
                 float myAlpha = dissolveTex.r;
                 float alpha = step(_MyTime,myAlpha);
                 clip(alpha - 0.01f);
                 float alphaStep = step(myAlpha,_MyTime + 0.03);
                 col.rgb += alphaStep * _DissolveColor;
                 col = half4(col.rgb,1.0);
                col.rgb = MixFog(col.rgb,i.fogCoord);
                return col;
            }
            ENDHLSL
        }
    }
}