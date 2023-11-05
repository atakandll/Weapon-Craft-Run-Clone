// Toony Colors Pro+Mobile 2
// (c) 2014-2021 Jean Moreno

Shader "Toony Colors Pro 2/User/Custom Toony Shader"
{
	Properties
	{
		[Enum(Front, 2, Back, 1, Both, 0)] _Cull ("Render Face", Float) = 2.0
		[TCP2ToggleNoKeyword] _ZWrite ("Depth Write", Float) = 1.0
		[HideInInspector] _RenderingMode ("rendering mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("blending source", Float) = 1.0
		[HideInInspector] _DstBlend ("blending destination", Float) = 0.0
		[TCP2Separator]

		//================================
		// Injected Code for 'Properties/Start'
		[TCP2HeaderToggle(_EMISSION)] _UseEmission ("Emission", Float) = 0
		_EmissionMap ("Texture (A)", 2D) = "white" {}
		[TCP2ColorNoAlpha] [HDR] _EmissionColor ("Emission Color", Color) = (1,1,0,1)
		[TCP2Separator]
		//================================

		[TCP2HeaderHelp(Base)]
		_BaseColor ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_BaseMap ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(Specular)]
		[Toggle(TCP2_SPECULAR)] _UseSpecular ("Enable Specular", Float) = 0
		[TCP2ColorNoAlpha] [HDR] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SpecularRoughness ("Roughness", Range(0,1)) = 0.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[Toggle(TCP2_RIM_LIGHTING)] _UseRim ("Enable Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] [HDR] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMinVert ("Rim Min", Range(0,2)) = 0.5
		_RimMaxVert ("Rim Max", Range(0,2)) = 1
		[TCP2Separator]
		
		[TCP2HeaderHelp(MatCap)]
		[Toggle(TCP2_MATCAP)] _UseMatCap ("Enable MatCap", Float) = 0
		[NoScaleOffset] _MatCapTex ("MatCap (RGB)", 2D) = "white" {}
		[TCP2ColorNoAlpha] _MatCapColor ("MatCap Color", Color) = (1,1,1,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Normal Mapping)]
		[Toggle(_NORMALMAP)] _UseNormalMap ("Enable Normal Mapping", Float) = 0
		[NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
		[TCP2Separator]
		
		// Injection Point: 'Properties/End'

		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			// Injection Point: 'SubShader/Tags'
		}

		// Injection Point: 'SubShader/Shader States'

		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Injection Point: 'Include Files'

		// Shader Properties
		sampler2D _BumpMap;
		sampler2D _BaseMap;
		
		// Shader Properties
		float _RimMinVert;
		float _RimMaxVert;
		float4 _BaseMap_ST;
		fixed4 _BaseColor;
		fixed4 _MatCapColor;
		float _RampThreshold;
		float _RampSmoothing;
		fixed4 _HColor;
		fixed4 _SColor;
		float _SpecularRoughness;
		half4 _SpecularColor;
		half4 _RimColor;

		// Injection Point: 'Variables/Outside CBuffer'
		//================================
		// Injected Code for 'Variables/Inside CBuffer'
		float4 _EmissionMap_ST;
		half4 _EmissionColor;
		sampler2D _EmissionMap;
		//================================

		sampler2D _MatCapTex;

		//Specular help functions (from UnityStandardBRDF.cginc)
		inline half3 SpecSafeNormalize(half3 inVec)
		{
			half dp3 = max(0.001f, dot(inVec, inVec));
			return inVec * rsqrt(dp3);
		}
		
		//GGX
		#define TCP2_PI 3.14159265359
		#define TCP2_INV_PI        0.31830988618f
		#if defined(SHADER_API_MOBILE)
			#define TCP2_EPSILON 1e-4f
		#else
			#define TCP2_EPSILON 1e-7f
		#endif
		inline half GGX(half NdotH, half roughness)
		{
			half a2 = roughness * roughness;
			half d = (NdotH * a2 - NdotH) * NdotH + 1.0f;
			return TCP2_INV_PI * a2 / (d * d + TCP2_EPSILON);
		}

		// Injection Point: 'Functions'

		ENDCG

		// Main Surface Shader
		Blend [_SrcBlend] [_DstBlend]
		Cull [_Cull]
		ZWrite [_ZWrite]
		// Injection Point: 'Main Pass/Shader States'

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha addshadow fullforwardshadows keepalpha
		#pragma target 3.0
		//================================
		// Injected Code for 'Main Pass/Pragma'
		#pragma shader_feature_local _EMISSION
		//================================

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature TCP2_SPECULAR
		#pragma shader_feature TCP2_RIM_LIGHTING
		#pragma shader_feature _ _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
		#pragma shader_feature TCP2_MATCAP
		#pragma shader_feature _NORMALMAP

		//================================================================
		// STRUCTS

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			half4 tangent : TANGENT;
			// Injection Point: 'Main Pass/Attributes'
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			half3 viewDir;
			half3 tangent;
			half3 worldNormal; INTERNAL_DATA
			half rim;
			float2 texcoord0;
			//================================
			// Injected Code for 'Main Pass/Surface Input'
			float2 rawTexcoord;
			//================================

		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			//================================
			// Injected Code for 'Main Pass/Vertex Shader/Start'
			output.rawTexcoord.xy = v.texcoord0.xy;
			//================================

			// Texture Coordinates
			output.texcoord0.xy = v.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
			// Shader Properties Sampling
			float __rimMinVert = ( _RimMinVert );
			float __rimMaxVert = ( _RimMaxVert );

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float4 clipPos = UnityObjectToClipPos(v.vertex);

			//Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			half3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			half ndv = abs(dot(viewDir, worldNormal));
			half ndvRaw = ndv;

			output.tangent = mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0)).xyz;
			#if defined(TCP2_RIM_LIGHTING)
			half rim = 1 - ndvRaw;
			rim = smoothstep(__rimMinVert, __rimMaxVert, rim);
			output.rim = rim;
			#endif

			// Injection Point: 'Main Pass/Vertex Shader/End'
		}

		//================================================================

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 worldNormal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;
			float3 normalTS;

			Input input;
			
			// Shader Properties
			float __rampThreshold;
			float __rampSmoothing;
			float3 __highlightColor;
			float3 __shadowColor;
			float __ambientIntensity;
			float __specularRoughnessPbr;
			float3 __specularColor;
			float3 __rimColor;
			float __rimStrength;
		};

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{
			// Injection Point: 'Main Pass/Surface Function/Start'

			input.worldNormal = WorldNormalVector(input, output.Normal);

			// Shader Properties Sampling
			float4 __normalMap = ( tex2D(_BumpMap, input.texcoord0.xy).rgba );
			float4 __albedo = ( tex2D(_BaseMap, input.texcoord0.xy).rgba );
			float4 __mainColor = ( _BaseColor.rgba );
			float __alpha = ( __albedo.a * __mainColor.a );
			float3 __matcapColor = ( _MatCapColor.rgb );
			output.__rampThreshold = ( _RampThreshold );
			output.__rampSmoothing = ( _RampSmoothing );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__ambientIntensity = ( 1.0 );
			output.__specularRoughnessPbr = ( _SpecularRoughness );
			output.__specularColor = ( _SpecularColor.rgb );
			output.__rimColor = ( _RimColor.rgb );
			output.__rimStrength = ( 1.0 );

			output.input = input;

			#if defined(_NORMALMAP)
			half4 normalMap = half4(0,0,0,0);
			normalMap = __normalMap;
			output.Normal = UnpackNormal(normalMap);
			output.normalTS = output.Normal;

			#endif

			half3 worldNormal = WorldNormalVector(input, output.Normal);
			output.worldNormal = worldNormal;

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;
			
			output.Albedo *= __mainColor.rgb;
			
			//MatCap
			#if defined(TCP2_MATCAP)
			half2 capCoord = mul((float3x3)UNITY_MATRIX_V, worldNormal).xy * 0.5 + 0.5;
			fixed3 matcap = tex2D(_MatCapTex, capCoord).rgb * __matcapColor;
			output.Albedo *= matcap;
			#endif
			//================================
			// Injected Code for 'Main Pass/Surface Function/End'
			
			half3 emission = half3(0,0,0);
			
			// Emission
			#if defined(_EMISSION)
				float2 rawTexcoord = input.rawTexcoord.xy;
				emission = _EmissionColor.rgb;
				half4 emissionMap = tex2D(_EmissionMap, rawTexcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw);
				emission *= emissionMap.rgb;
				output.Emission += emission.rgb;
			#endif
			//================================

		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, half3 viewDir, UnityGI gi)
		{
			// Injection Point: 'Main Pass/Lighting Function/Start'
			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				//extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			//Apply attenuation (shadowmaps & point/spot lights attenuation)
			ndl *= atten;
			half3 ramp;
			
			// Wrapped Lighting
			ndl = ndl * 0.5 + 0.5;
			
			#define		RAMP_THRESHOLD	surface.__rampThreshold
			#define		RAMP_SMOOTH		surface.__rampSmoothing
			ndl = saturate(ndl);
			ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, ndl);

			//Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			//Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = 1;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				half3 ambient = gi.indirect.diffuse;
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif

			// Premultiply blending
			#if defined(_ALPHAPREMULTIPLY_ON)
				color.rgb *= color.a;
			#endif

			#if defined(TCP2_SPECULAR)
			//Specular: GGX
			half3 halfDir = SpecSafeNormalize(lightDir + viewDir);
			half roughness = surface.__specularRoughnessPbr*surface.__specularRoughnessPbr;
			half nh = saturate(dot(normal, halfDir));
			half spec = GGX(nh, saturate(roughness));
			spec *= TCP2_PI * 0.05;
			#ifdef UNITY_COLORSPACE_GAMMA
				spec = max(0, sqrt(max(1e-4h, spec)));
				half surfaceReduction = 1.0 - 0.28 * roughness * surface.__specularRoughnessPbr;
			#else
				half surfaceReduction = 1.0 / (roughness*roughness + 1.0);
			#endif
			spec = max(0, spec * ndl);
			spec *= surfaceReduction;
			spec *= atten;
			
			//Apply specular
			color.rgb += spec * lightColor.rgb * surface.__specularColor;
			#endif
			// Rim Lighting
			#if defined(TCP2_RIM_LIGHTING)
			half rim = surface.input.rim;
			rim = ( rim );
			half3 rimColor = surface.__rimColor;
			half rimStrength = surface.__rimStrength;
			//Rim light mask
			color.rgb += ndl * atten * rim * rimColor * rimStrength;
			#endif

			// Injection Point: 'Main Pass/Lighting Function/End'

			// Apply alpha to Forward Add passes
			#if defined(_ALPHABLEND_ON) && defined(UNITY_PASS_FORWARDADD)
				color.rgb *= color.a;
			#endif

			return color;
		}

		// Same as UnityGI_Base but with attenuation extraction that works with lightmaps
		inline UnityGI UnityGI_Base_TCP2(UnityGIInput data, half occlusion, half3 normalWorld, out half tcp2_atten)
		{
			UnityGI o_gi;
			ResetUnityGI(o_gi);

			// Base pass with Lightmap support is responsible for handling ShadowMask / blending here for performance reason
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
				half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
				float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
				float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
				data.atten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif

			o_gi.light = data.light;

			// TCP2: don't apply attenuation to light color
			// o_gi.light.color *= data.atten;

			// TCP2: extract attenuation
			tcp2_atten = data.atten;

			#if UNITY_SHOULD_SAMPLE_SH
				o_gi.indirect.diffuse = ShadeSHPerPixel(normalWorld, data.ambient, data.worldPos);
			#endif

			#if defined(LIGHTMAP_ON)
				// Baked lightmaps
				half4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, data.lightmapUV.xy);
				half3 bakedColor = DecodeLightmap(bakedColorTex);

				#ifdef DIRLIGHTMAP_COMBINED
					fixed4 bakedDirTex = UNITY_SAMPLE_TEX2D_SAMPLER (unity_LightmapInd, unity_Lightmap, data.lightmapUV.xy);
					o_gi.indirect.diffuse += DecodeDirectionalLightmap (bakedColor, bakedDirTex, normalWorld);

					#if defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN)
						ResetUnityLight(o_gi.light);
						o_gi.indirect.diffuse = SubtractMainLightWithRealtimeAttenuationFromLightmap (o_gi.indirect.diffuse, data.atten, bakedColorTex, normalWorld);
					#endif

				#else // not directional lightmap
					o_gi.indirect.diffuse += bakedColor;

					#if defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN)
						ResetUnityLight(o_gi.light);
						o_gi.indirect.diffuse = SubtractMainLightWithRealtimeAttenuationFromLightmap(o_gi.indirect.diffuse, data.atten, bakedColorTex, normalWorld);
					#endif

				#endif
			#endif

			#ifdef DYNAMICLIGHTMAP_ON
				// Dynamic lightmaps
				fixed4 realtimeColorTex = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, data.lightmapUV.zw);
				half3 realtimeColor = DecodeRealtimeLightmap (realtimeColorTex);

				#ifdef DIRLIGHTMAP_COMBINED
					half4 realtimeDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicDirectionality, unity_DynamicLightmap, data.lightmapUV.zw);
					o_gi.indirect.diffuse += DecodeDirectionalLightmap (realtimeColor, realtimeDirTex, normalWorld);
				#else
					o_gi.indirect.diffuse += realtimeColor;
				#endif
			#endif

			o_gi.indirect.diffuse *= occlusion;
			return o_gi;
		}

		inline UnityGI UnityGlobalIllumination_TCP2 (UnityGIInput data, half occlusion, half3 normalWorld, out half tcp2_atten)
		{
			return UnityGI_Base_TCP2(data, occlusion, normalWorld, tcp2_atten);
		}

		inline UnityGI UnityGlobalIllumination_TCP2 (UnityGIInput data, half occlusion, half3 normalWorld, Unity_GlossyEnvironmentData glossIn, out half tcp2_atten)
		{
			UnityGI o_gi = UnityGI_Base_TCP2(data, occlusion, normalWorld, tcp2_atten);
			o_gi.indirect.specular = UnityGI_IndirectSpecular(data, occlusion, glossIn);
			return o_gi;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			// Injection Point: 'Main Pass/GI Function/Start'
			half3 normal = surface.Normal;

			//GI without reflection probes
			half tcp2_atten;
			gi = UnityGlobalIllumination_TCP2(data, 1.0, normal, tcp2_atten); // occlusion is applied in the lighting function, if necessary

			surface.atten = tcp2_atten; // transfer attenuation to lighting function

			// Injection Point: 'Main Pass/GI Function/End'
		}

		ENDCG

	}

	Fallback "Diffuse"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2021.3.17f1";ver:"2.7.0";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","SPEC_PBR_GGX","SPECULAR","RIM","RIM_SHADER_FEATURE","ENABLE_FOG","ENABLE_LIGHTMAPS","ENABLE_LPPV","RIM_VERTEX","RIM_LIGHTMASK","BUMP","BUMP_SHADER_FEATURE","SPECULAR_SHADER_FEATURE","WRAPPED_LIGHTING_HALF","ATTEN_AT_NDL","UNITY_2020_1","AUTO_TRANSPARENT_BLENDING","SS_SHADER_FEATURE","UNITY_2021_1","MATCAP_MULT","MATCAP","MATCAP_SHADER_FEATURE","MATCAP_PIXEL","MATCAP_PERSPECTIVE_CORRECTION"];flags:list["addshadow","fullforwardshadows"];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",RIM_LABEL="Rim Lighting"];shaderProperties:list[sp(name:"Albedo";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:True;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:4;chan:"RGBA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_BaseMap";md:"";gbv:False;custom:False;refs:"";guid:"6ec32036-9c14-48d2-8f6b-378d7f117836";op:Multiply;lbl:"Albedo";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),sp(name:"Main Color";imps:list[imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:False;cc:4;chan:"RGBA";prop:"_BaseColor";md:"";gbv:False;custom:False;refs:"";guid:"9dab31ee-6b7b-40d9-98f7-80cfe52ff15b";op:Multiply;lbl:"Color";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,,,,,sp(name:"Specular Color";imps:list[imp_mp_color(def:RGBA(0.5, 0.5, 0.5, 1);hdr:True;cc:3;chan:"RGB";prop:"_SpecularColor";md:"";gbv:False;custom:False;refs:"";guid:"d11860f4-e2e7-4ae4-8ea5-922371f03a69";op:Multiply;lbl:"Specular Color";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),sp(name:"Specular Roughness PBR";imps:list[imp_mp_range(def:0.5;min:0;max:1;prop:"_SpecularRoughness";md:"";gbv:False;custom:False;refs:"";guid:"40c12467-2a8c-4b98-a298-4752d54c5530";op:Multiply;lbl:"Roughness";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),sp(name:"Rim Color";imps:list[imp_mp_color(def:RGBA(0.8, 0.8, 0.8, 0.5);hdr:True;cc:3;chan:"RGB";prop:"_RimColor";md:"";gbv:False;custom:False;refs:"";guid:"86f0ea44-c8c6-49a8-8710-e583a6018c19";op:Multiply;lbl:"Rim Color";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,,,,,,,,,,,,,,,,,,,,,sp(name:"Emission";imps:list[imp_mp_color(def:RGBA(0, 0, 0, 1);hdr:True;cc:3;chan:"RGB";prop:"_EmissionColor";md:"";gbv:False;custom:False;refs:"";guid:"7f5c657a-5ff6-4faa-935b-324789fba4d1";op:Multiply;lbl:"Emission Color";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False)];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[injectedFile(guid:"703d4b59eb16e7542852376913d859ad";filename:"Custom_Toony_Shader_Injection";injectedPoints:list[injectedPoint(name:"Properties/Start";enabled:True;replace:False;displayName:__NULL__;blockName:"Define Custom Properties";program:Undefined;shaderProperties:list[]),injectedPoint(name:"Variables/Inside CBuffer";enabled:True;replace:False;displayName:__NULL__;blockName:"Declare Custom Properties";program:Undefined;shaderProperties:list[]),injectedPoint(name:"Main Pass/Pragma";enabled:True;replace:False;displayName:__NULL__;blockName:"Custom Shader Features";program:Undefined;shaderProperties:list[]),injectedPoint(name:"Main Pass/Surface Input";enabled:True;replace:False;displayName:__NULL__;blockName:"Define Raw Texcoord";program:Undefined;shaderProperties:list[]),injectedPoint(name:"Main Pass/Vertex Shader/Start";enabled:True;replace:False;displayName:__NULL__;blockName:"Get Raw Texcoord";program:Vertex;shaderProperties:list[]),injectedPoint(name:"Main Pass/Surface Function/End";enabled:True;replace:False;displayName:__NULL__;blockName:"Add Emission Color";program:Fragment;shaderProperties:list[])])];mark:True);matLayers:list[]) */
/* TCP_HASH 9eeda2dfc28e363d6e8458ac78a6d252 */
