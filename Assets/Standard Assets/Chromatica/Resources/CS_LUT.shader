Shader "Hidden/Chroma/LUT"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Contribution ("Contribution (Float)", Range(0, 1)) = 1.0

		_LookupTex ("LUT (RGB)", 2D) = "white" {}
		_LookupTexDepth ("Depth LUT (RGB)", 2D) = "white" {}
		_DepthCurve ("Depth Curve (Alpha)", 2D) = "white" {}
		_LutParams ("Scale (XY) Offset (Z)", Vector) = (0, 0, 0, 0)

		_Exposure ("Tonemapping Exposure (Float)", Float) = 16.0

		_MaskOpacity ("Screen Mask Opacity (Float)", Range(0, 1)) = 1.0
		_Mask ("Screen Mask (RGB)", 2D) = "white" {}
		_MaskInvert ("Screen Mask Invert (Float)", Float) = 0.0
		_MaskTilingOffset ("Screen Mask Tiling (XY) Offset (ZW)", Vector) = (1.0, 1.0, 0.0, 0.0)
	}

	CGINCLUDE
	
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma exclude_renderers flash
		#pragma glsl
		#pragma target 3.0
		#pragma multi_compile __ CHROMA_TONEMAPPING
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half _Contribution;
		sampler2D _LookupTex;
		sampler2D _LookupTexDepth;
		sampler2D _DepthCurve;
		sampler2D _Mask;
		half3 _LutParams;
		half _Exposure;
		half _MaskOpacity;
		half _MaskInvert;
		half4 _MaskTilingOffset;

		sampler2D_float _CameraDepthTexture;
		float4 _CameraDepthTexture_ST;
		float4 _MainTex_TexelSize;
		
		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv  : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
		};

		struct v2f2
		{
			float4 pos : SV_POSITION;
			float2 uv  : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
			float2 uv3 : TEXCOORD2;
		};

		v2f vert_dual(appdata_img v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv =  v.texcoord.xy;
			o.uv2 = TRANSFORM_TEX(v.texcoord, _CameraDepthTexture);
		
			#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0.0)
				o.uv2.y = 1.0 - o.uv2.y;
			#endif
		
			return o;
		}

		v2f vert_masked(appdata_img v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv =  v.texcoord.xy;
			o.uv2 = o.uv;
		
			#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0.0)
				o.uv2.y = 1.0 - o.uv2.y;
			#endif
		
			return o;
		}

		v2f2 vert_masked_dual(appdata_img v)
		{
			v2f2 o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv =  v.texcoord.xy;
			o.uv2 = TRANSFORM_TEX(v.texcoord, _CameraDepthTexture);
			o.uv3 = o.uv;
		
			#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0.0)
			{
				o.uv2.y = 1.0 - o.uv2.y;
				o.uv3.y = 1.0 - o.uv3.y;
			}
			#endif
		
			return o;
		}

		half4 tonemap(half4 color)
		{
			#if CHROMA_TONEMAPPING
			color.rgb *= _Exposure;
			half3 x = max(half3(0.0, 0.0, 0.0), color.rgb - 0.004);
			half3 o = (x * (6.2 * x + 0.5)) / (x * (6.2 * x + 1.7) + 0.06);
			return half4(o * o, color.a);
			#else
			return saturate(color);
			#endif
		}

		// sRGB <-> Linear from http://entropymine.com/imageworsener/srgbformula/
		// using a bit more precise values than the IEC 61966-2-1 standard
		// see http://en.wikipedia.org/wiki/SRGB for more information
		half4 sRGB(half4 color)
		{
			color.rgb = (color.rgb <= half3(0.0031308, 0.0031308, 0.0031308)) ? color.rgb * 12.9232102 : 1.055 * pow(color.rgb, 0.41666) - 0.055;
			return color;
		}

		half4 Linear(half4 color)
		{
			color.rgb = (color <= half3(0.0404482, 0.0404482, 0.0404482)) ? color.rgb / 12.9232102 : pow((color.rgb + 0.055) * 0.9478672, 2.4);
			return color;
		}
		// ...
		
		half4 LUT(half4 uv)
		{
			half4 o = uv;
			uv.y = 1.0 - uv.y;
			uv.z *= _LutParams.z;
			half shift = floor(uv.z);
			uv.xy = uv.xy * _LutParams.z * _LutParams.xy + 0.5 * _LutParams.xy;
			uv.x += shift * _LutParams.y;
			uv.xyz = lerp(tex2D(_LookupTex, uv.xy).rgb, tex2D(_LookupTex, uv.xy + half2(_LutParams.y, 0)).rgb, uv.z - shift);
			return lerp(o, uv, _Contribution);
		}

		half4 DualLUT(half2 uv2, half4 color)
		{
			half4 o = color;

			half3 coord = color.rgb;
			coord.y = 1.0 - coord.y;
			coord.z *= _LutParams.z;
			half shift = floor(coord.z);
			coord.xy = coord.xy * _LutParams.z * _LutParams.xy + 0.5 * _LutParams.xy;
			coord.x += shift * _LutParams.y;
			half l = coord.z - shift;
			half2 coordOffset = coord.xy + half2(_LutParams.y, 0);

			half3 colorNear = lerp(tex2D(_LookupTex, coord.xy).rgb, tex2D(_LookupTex, coordOffset).rgb, l);
			half3 colorFar = lerp(tex2D(_LookupTexDepth, coord.xy).rgb, tex2D(_LookupTexDepth, coordOffset).rgb, l);
			
			half d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv2);
			half d10 = tex2D(_DepthCurve, half2(Linear01Depth(d), 0.5)).a;
			half3 finalColor = lerp(colorNear, colorFar, d10);
			return lerp(o, half4(finalColor, o.a), _Contribution);
		}

		half4 Mask(half2 uv, half4 color, half4 correctedColor)
		{
			half3 m = abs(_MaskInvert - tex2D(_Mask, uv * _MaskTilingOffset.xy + _MaskTilingOffset.zw).rgb) * _MaskOpacity;
			return half4(lerp(correctedColor.rgb, color.rgb, m), color.a);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Normal - GAMMA
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return LUT(color);
				}

			ENDCG
		}

		// (1) Masked - GAMMA
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return Mask(i.uv2, color, LUT(color));
				}

			ENDCG
		}

		// (2) Horizontal Split - GAMMA
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return i.uv.x > 0.5 ? LUT(color) : color;
				}

			ENDCG
		}

		// (3) Vertical Split - GAMMA
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return i.uv.y < 0.5 ? LUT(color) : color;
				}

			ENDCG
		}

		// (4) Normal (Dual) - GAMMA
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return DualLUT(i.uv2, color);
				}

			ENDCG
		}

		// (5) Masked (Dual) - GAMMA
		Pass
		{
			CGPROGRAM

				half4 frag(v2f2 i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return Mask(i.uv3, color, DualLUT(i.uv2, color));
				}

			ENDCG
		}

		// (6) Horizontal Split (Dual) - GAMMA
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return i.uv.x > 0.5 ? DualLUT(i.uv2, color) : color;
				}

			ENDCG
		}

		// (7) Vertical Split (Dual) - GAMMA
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return i.uv.y < 0.5 ? DualLUT(i.uv2, color) : color;
				}

			ENDCG
		}

		// --------------------------------------------------------------------------------
		//                                    LINEAR
		// --------------------------------------------------------------------------------

		// (8) Normal - LINEAR
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = sRGB(tonemap(tex2D(_MainTex, i.uv)));
					return Linear(LUT(color));
				}

			ENDCG
		}

		// (9) Masked - LINEAR
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f i) : SV_Target
				{
					half4 orig = tonemap(tex2D(_MainTex, i.uv));
					half4 color = sRGB(orig);
					return Mask(i.uv2, orig, Linear(LUT(color)));
				}

			ENDCG
		}

		// (10) Horizontal Split - LINEAR
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tonemap((tex2D(_MainTex, i.uv)));
					return i.uv.x > 0.5 ? Linear(LUT(sRGB(color))) : color;
				}

			ENDCG
		}

		// (11) Vertical Split - LINEAR
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tonemap((tex2D(_MainTex, i.uv)));
					return i.uv.y < 0.5 ? Linear(LUT(sRGB(color))) : color;
				}

			ENDCG
		}

		// (12) Normal (Dual) - LINEAR
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f i) : SV_Target
				{
					half4 color = sRGB(tonemap(tex2D(_MainTex, i.uv)));
					return Linear(DualLUT(i.uv2, color));
				}

			ENDCG
		}

		// (12) Masked (Dual) - LINEAR
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f2 i) : SV_Target
				{
					half4 orig = tonemap(tex2D(_MainTex, i.uv));
					half4 color = sRGB(orig);
					return Mask(i.uv3, orig, Linear(DualLUT(i.uv2, color)));
				}

			ENDCG
		}

		// (13) Horizontal Split (Dual) - LINEAR
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return i.uv.x > 0.5 ? Linear(DualLUT(i.uv2, sRGB(color))) : color;
				}

			ENDCG
		}

		// (14) Vertical Split (Dual) - LINEAR
		Pass
		{			
			CGPROGRAM

				half4 frag(v2f i) : SV_Target
				{
					half4 color = tonemap(tex2D(_MainTex, i.uv));
					return i.uv.y < 0.5 ? Linear(DualLUT(i.uv2, sRGB(color))) : color;
				}

			ENDCG
		}
	}

	FallBack off
}
